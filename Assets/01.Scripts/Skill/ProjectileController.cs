using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;

// 생성된 Projectile 움직임을 정하는 클래스
// 생성 된 후 코루틴을 돌려 파괴될 때 까지 계속 지정한 대로 움직임
public class ProjectileController : SkillBase
{
    PlayerController _owner;
    SkillBase Skill;
    Vector2 _spawnPos;
    Vector3 _dir = Vector3.zero;
    Vector3 _target = Vector3.zero;
    SkillType _skillType;
    Rigidbody2D _rigid;
    int _numPenerations;
    public int _bounceCount = 1;
    public float _projectileSpeed = 1;
    public float _attackInterval;

    GameObject _meteorShadow;

    List<MonsterBase> _enteredColliderList = new List<MonsterBase>();
    Coroutine _coDotDamage;

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(PlayerController owner, Vector2 position, Vector2 dir, Vector2 target, SkillBase skill, int skillLevel)
    {
        _owner = owner;
        _spawnPos = position;
        _dir = dir;
        Skill = skill;
        _rigid = GetComponent<Rigidbody2D>();

        _target = target;
        Debug.Log("스킬레벨체크");
        transform.localScale = Vector3.one * Skill.SkillData.skillLevel[skillLevel].ScaleMultiplier;
        _numPenerations = Skill.SkillData.skillLevel[skillLevel].NumPenerations;
        _bounceCount = Skill.SkillData.skillLevel[skillLevel].NumBounce;
        _projectileSpeed = Skill.SkillData.skillLevel[skillLevel].projectileSpeed;
        _attackInterval = Skill.SkillData.skillLevel[skillLevel].AttackInterval;

        switch (Skill.SkillType)
        {
            case SkillType.HolyProjectile:
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(CoArrow(skillLevel));
                }
                break;
            case SkillType.HolyPulse:
                StartCoroutine(CoHolyPulse(skillLevel));
                break;
            case SkillType.WindCutter:
                if (gameObject.activeInHierarchy)
                    StartCoroutine(CoBoomerang(skillLevel));
                break;
            case SkillType.BloodChain:
                StartCoroutine(CoBloodChain(_spawnPos, _target, true));
                break;
                case SkillType.Meteor:
                _dir = (_target - transform.position).normalized;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
                _rigid.velocity = _dir * Skill.SkillData.skillLevel[skillLevel].projectileSpeed;
                // 메테오 착단 지점
                _meteorShadow = ResourceManager.Instance.Instantiate("MeteorShadow", pooling: true);
                _meteorShadow.transform.position = target; //+ new Vector3(-0.5f, -0.45f, 1);
                if (gameObject.activeInHierarchy)
                    StartCoroutine(CoMeteor());
                break;

            default:
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
                _numPenerations = Skill.SkillData.skillLevel[skillLevel].NumPenerations;
                _rigid.velocity = _dir * Skill.SkillData.skillLevel[skillLevel].projectileSpeed;
                break;
        }

        if (gameObject.activeInHierarchy)
            StartCoroutine(CoCheckDestory());

    }

    private float _rotateAmount = 1000;

    IEnumerator CoMeteor()
    {
        while (true)
        {
            if (_meteorShadow != null)
            {
                Vector2 shadowPosition = _meteorShadow.transform.position;

                float distance = Vector2.Distance(shadowPosition, transform.position);
                float scale = Mathf.Lerp(0f, 2.5f, 1 - distance / 10f);
                _meteorShadow.transform.position = shadowPosition;
                _meteorShadow.transform.localScale = new Vector3(scale, scale, 1f);
            }
            if (Vector2.Distance(_rigid.position, _target) < 0.3f)
                ExplosionMeteor();
            yield return new WaitForFixedUpdate();
        }
    }
    
    void ExplosionMeteor()
    {
        ResourceManager.Instance.Destroy(_meteorShadow);
        float scanRange = 1.5f;
        string prefabName = "FireExplosion";
        GameObject obj =ResourceManager.Instance.Instantiate(prefabName, pooling : true);
        obj.transform.position = transform.position;

        RaycastHit2D[] _targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0);

        foreach (RaycastHit2D _target in _targets)
        {
            MonsterBase creature = _target.transform.GetComponent<MonsterBase>();
            if (!creature.IsValid())
                return;
            creature.TakeDamage(Skill.TotalDamage);
        }

        DestroyProjectile();
    }


    IEnumerator CoHolyPulse(int skillLevel)
    {
        Debug.Log("스킬레벨체크");
        Vector3 targePoint = GameManager.Instance.controller.transform.position + _dir * Skill.SkillData.skillLevel[skillLevel].projectileSpeed * 3;

        Sequence seq = DOTween.Sequence();


        seq.Append(transform.DOMove(targePoint, 5f).SetEase(Ease.Linear));

        yield return new WaitForSeconds(3f/*Skill.SkillData.Duration*/);

        while (true)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, GameManager.Instance.controller.transform.position, Time.deltaTime * Skill.SkillData.skillLevel[skillLevel].projectileSpeed * 4f);
            if (GameManager.Instance.controller.transform.position == transform.position)
            {
                DestroyProjectile();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator CoBloodChain(Vector3 startPos, Vector3 endPos, bool isFollow = false)
    {
        SetParticleSize(startPos, endPos);
        yield return new WaitForSeconds(0.25f);
        DestroyProjectile();
    }
    void SetParticleSize(Vector3 startPos, Vector3 endPos)
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        var main = particle.main;

        // Scale
        transform.position = startPos;
        float dist = Vector3.Distance(startPos, endPos);
        main.startSizeX  = dist;
        main.startSizeY = 1;
        // rotatate
        Vector3 dir = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        main.startRotation  = angle * -1f;

        // Cast box
        List<Transform> listMonster = new List<Transform>();
        LayerMask targetLayer = LayerMask.GetMask("Monster");
        float boxWidth = 1f;
        Vector3 midPos = (startPos + endPos) / 2f;
        Vector2 boxSize = new Vector2(boxWidth, boxWidth);
        float angleRad = angle * Mathf.Deg2Rad;

        RaycastHit2D[] colliders = Physics2D.BoxCastAll(midPos, boxSize, 0, dir, dist * 1.3f, targetLayer); 

        foreach (RaycastHit2D hit in colliders)
        {
            MonsterBase monster = hit.transform.GetComponent<MonsterBase>();
            if (monster != null)
            {
                monster.TakeDamage(Skill.TotalDamage);
            }
        }
    }


    IEnumerator CoArrow(int skillLevel)
    {
        Vector2 direction = (Vector2)GameManager.Instance.controller.transform.position + new Vector2(5, 0) - _rigid.position;
        float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
        _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
        _rigid.velocity = transform.up * Skill.SkillData.skillLevel[skillLevel].projectileSpeed;

        yield return new WaitForFixedUpdate();

    }

    IEnumerator CoBoomerang(int skillLevel)
    {
        Vector3 targePoint = GameManager.Instance.controller.transform.position + _dir * Skill.SkillData.skillLevel[skillLevel].projectileSpeed;
        transform.localScale = Vector3.zero;
        transform.localScale = Vector3.one * Skill.SkillData.skillLevel[skillLevel].ScaleMultiplier;

        Sequence seq = DOTween.Sequence();

        float projectileTravelTime = 1f;
        float secondSeqStartTime = 0.7f;
        float secondSeqDuringTime = 1.8f;

        seq.Append(transform.DOMove(targePoint, projectileTravelTime).SetEase(Ease.OutExpo))
            .Insert(secondSeqStartTime, transform.DOMove(targePoint + _dir, secondSeqDuringTime).SetEase(Ease.Linear));

        yield return new WaitForSeconds(3f/*Skill.SkillData.Duration*/);

        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.controller.transform.position, Time.deltaTime * Skill.SkillData.skillLevel[skillLevel].projectileSpeed * 4f);
            if (GameManager.Instance.controller.transform.position == transform.position)
            {
                DestroyProjectile();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }



    void BounceProjectile(MonsterBase creature, int skillLevel)
    {
        List<Transform> list = new List<Transform>();
        list = ObjectManager.Instance.GetFindMonstersInFanShape(creature.transform.position, _dir, 5.5f, 240);

        List<Transform> sortedList = (from t in list orderby Vector3.Distance(t.position, transform.position) descending select t).ToList();

        if (sortedList.Count == 0)
        {
            DestroyProjectile();
        }
        else
        {
            int index = Random.Range(sortedList.Count / 2, sortedList.Count);
            _dir = (sortedList[index].position - transform.position).normalized;
            _rigid.velocity = _dir * Skill.SkillData.skillLevel[skillLevel].BounceSpeed;
        }
    }



    IEnumerator CoCheckDestory()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            DestroyProjectile();
        }
    }

    IEnumerator CoStartDotDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            // 몬스터도 스킬 쏘면 바꿀 것
            foreach (MonsterBase target in _enteredColliderList)
            {
                target.TakeDamage(Skill.TotalDamage);
            }
        }
    }

    IEnumerator CoDestroy(int skillLevel)
    {
        yield return new WaitForSeconds(Skill.SkillData.skillLevel[skillLevel].Duration);
        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        SkillManager.Instance.DespawnProjectile(this);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterBase creature = collision.transform.GetComponent<MonsterBase>();
         if (creature.IsValid() == false)
             return;

        if (this.IsValid() == false)
            return;

        switch (Skill.SkillType)
        {
            case SkillType.HolyProjectile:
                _numPenerations--;
                if (_numPenerations < 0)
                {
                    _rigid.velocity = Vector3.zero;
                    //SpawnHolyImpact(Skill.transform.position);
                    DestroyProjectile();
                }
                break;
            case SkillType.HolyPulse:
                _numPenerations--;
                if (_numPenerations < 0)
                {
                    _rigid.velocity = Vector3.zero;
                    DestroyProjectile();
                }
                break;
            case SkillType.PoisionBomb:
                _enteredColliderList.Add(creature);
                if (_coDotDamage == null)
                    _coDotDamage = StartCoroutine(CoStartDotDamage());
                break;
            case SkillType.DarkArrow:
                _bounceCount--;
                if (_bounceCount < 0)
                {
                    _rigid.velocity = Vector3.zero;
                    DestroyProjectile();
                }
                break;
            case SkillType.WindCutter:
                _enteredColliderList.Add(creature);
                if (_coDotDamage == null)
                    _coDotDamage = StartCoroutine(CoStartDotDamage());
                break;
            default:
                break;
        }
        creature.TakeDamage(Skill.TotalDamage);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        MonsterBase target = collision.transform.GetComponent<MonsterBase>();
         if (target.IsValid() == false)
             return;

        if (this.IsValid() == false)
            return;

        _enteredColliderList.Remove(target);

        if (_enteredColliderList.Count == 0 && _coDotDamage != null)
        {
            StopCoroutine(_coDotDamage);
            _coDotDamage = null;
        }
    }


    // 스킬 이펙트
    // 따로 클래스를 만들기엔 하나뿐이라 이렇게 빼둠.
    // HolyProjectile에서 직접 연결해주기
    // #region HolyImpact
    // public GameObject holyImpact_Prefab;

    // public ProjectileController SpawnHolyImpact(Vector2 position)
    // {
    //     GameObject go = PoolManager.Instance.Pop(holyImpact_Prefab);
    //     go.transform.position = position;

    //     ProjectileController sc = go.GetComponent<ProjectileController>();
    //     sc.Init();

    //     if (gameObject.activeInHierarchy)
    //         StartCoroutine(CoCheckDestory());
    //     return sc;
    // }

    // public void DeSpawnHolyImpact(ProjectileController go)
    // {
    //     PoolManager.Instance.Push(go.gameObject);
    // }
    // #endregion
}