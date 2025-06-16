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
    // 혹시 나중에 몬스터도 쏘면 바꿀 것
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

    // 나중에 몬스터도 스킬 쏘면 바꿀 것
    List<MonsterBase> _enteredColliderList = new List<MonsterBase>();
    Coroutine _coDotDamage;
    List<Transform> _chainLightningList = new List<Transform>();

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

    public void SetInfo(PlayerController owner, Vector2 position, Vector2 dir, Vector2 target, SkillBase skill)
    {
        _owner = owner;
        _spawnPos = position;
        _dir = dir;
        Skill = skill;
        _rigid = GetComponent<Rigidbody2D>();

        _target = target;
        transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;
        _numPenerations = Skill.SkillData.NumPenerations;
        _bounceCount = Skill.SkillData.NumBounce;
        _projectileSpeed = Skill.SkillData.projectileSpeed;
        _attackInterval = Skill.SkillData.AttackInterval;

        switch (Skill.SkillType)
        {
            case SkillType.HolyProjectile:
                if (gameObject.activeInHierarchy)
                {
                 StartCoroutine(CoBoomerang());
                }
                //StartCoroutine(CoTestSkill());
                    break;

            // case SkillType.HolyProjectile:
            //     if(gameObject.activeInHierarchy)
            //     StartCoroutine(CoHolyProjectile());

            //     break;
            case SkillType.HolyPulse:
                if (gameObject.activeInHierarchy)
                    StartCoroutine(CoFireSkill());
                break;
            case SkillType.FireSkill:
                if (gameObject.activeInHierarchy)
                    StartCoroutine(CoFireSkill());
                break;
            case SkillType.IceSkill:
                StartCoroutine(CoIceSkill());
                break;
            default:
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
                _numPenerations = Skill.SkillData.NumPenerations;
                _rigid.velocity = _dir * SkillData.projectileSpeed;
                break;
        }

        if (gameObject.activeInHierarchy)
            StartCoroutine(CoCheckDestory());

    }

    float _timer = 0;
    private float _rotateAmount = 1000;

    IEnumerator CoBoomerang()
    {
        Vector3 targePoint = GameManager.Instance.controller.transform.position + _dir * Skill.SkillData.projectileSpeed;
        transform.localScale = Vector3.zero;
        transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;

        Sequence seq = DOTween.Sequence();

        float projectileTravelTime = 1f;
        float secondSeqStartTime = 0.7f;
        float secondSeqDuringTime = 1.8f;

        seq.Append(transform.DOMove(targePoint, projectileTravelTime).SetEase(Ease.OutExpo))
            .Insert(secondSeqStartTime, transform.DOMove(targePoint + _dir, secondSeqDuringTime).SetEase(Ease.Linear));

        yield return new WaitForSeconds(3f/*Skill.SkillData.Duration*/);

        while (true)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, GameManager.Instance.controller.transform.position, Time.deltaTime * Skill.SkillData.projectileSpeed * 4f);
            if (GameManager.Instance.controller.transform.position == transform.position)
            {
                DestroyProjectile();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoTestSkill()
    {
        while (true)
        {
            gameObject.transform.position += Vector3.forward* Time.deltaTime * SkillData.projectileSpeed;
            yield return new WaitForSeconds(_attackInterval);
        }
    }

    IEnumerator CoHolyProjectile()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        // 아이템 데이터를 올바르게 바꿔야할 수도...
        string prefabName = SkillData.name;
        Vector3 position = GameManager.Instance.controller.transform.position;
        while (true)
        {

            for (int i = 0; i < SkillData.projectileCount; i++)
            {
                Vector3 dir = GameManager.Instance.MoveDir/*-(position - indicator.transform.position)*/.normalized;


                GenerateProjectile(GameManager.Instance.controller, prefabName, position, dir, GameManager.Instance.controller.transform.position, SkillData.SkillPrefabs);

                yield return wait;//new WaitForSeconds(SkillData.AttackInterval);

            }
        }
    }

    IEnumerator TestHoly()
    {

        List<MonsterBase> target = ObjectManager.Instance.GetMonsterWithinCamera(1);
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > 3 || target == null)
            {
                DestroyProjectile();
                _timer = 0;
                break;
            }

            // if (target[0].IsValid() == false)
            //     break;

            Vector2 direction = (Vector2)target[0].transform.position - _rigid.position;
            float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
            _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
            _rigid.velocity = transform.up * Skill.SkillData.projectileSpeed;

            //if (Vector2.Distance(_rigid.position, targetPos) < 0.3f)
            //    ExplosionMeteor();
            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator CoIceKill()
    {
        List<MonsterBase> target = ObjectManager.Instance.GetMonsterWithinCamera(1);
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > 3 || target == null)
            {
                DestroyProjectile();
                _timer = 0;
                break;
            }

            Vector2 direction = (Vector2)target[0].transform.position - _rigid.position;
            float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
            _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
            _rigid.velocity = transform.up * Skill.SkillData.projectileSpeed;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoChainLightning(Vector3 startPos, Vector3 endPos, bool isFollow = false)
    {
        SetParticleSize(startPos, endPos);
        yield return new WaitForSeconds(0.25f);
        DestroyProjectile();
    }

    void SetParticleSize(Vector3 startPos, Vector3 endPos)
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        ParticleSystem childParticle = Util.FindChild<ParticleSystem>(gameObject);//Util.FindChild<ParticleSystem>(gameObject);
        var main = particle.main;
        var main2 = childParticle.main;

        // Scale
        transform.position = startPos;
        float dist = Vector3.Distance(startPos, endPos);
        main.startSizeX = main2.startSizeX = dist;
        main.startSizeY = main2.startSizeY = 8;
        // rotatate
        Vector3 dir = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        main.startRotation = main2.startRotation = angle * -1f;

        // Cast box
        List<Transform> listMonster = new List<Transform>();
        LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
        float boxWidth = 1f;
        Vector3 midPos = (startPos + endPos) / 2f; // �������� ���� ������ �߰� ����
        Vector2 boxSize = new Vector2(boxWidth, boxWidth);
        //Vector2 boxSize = new Vector2(300, 399);
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

    IEnumerator CoFireSkill()
    {
        Vector3 targePoint = GameManager.Instance.player.transform.position + _dir * Skill.SkillData.projectileSpeed;
        transform.localScale = Vector3.zero;
        transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;

        Sequence seq = DOTween.Sequence();

        float projectileTravelTime = 1f;
        float secondSeqStartTime = 0.7f;
        float secondSeqDuringTime = 1.8f;

        seq.Append(transform.DOMove(targePoint, projectileTravelTime).SetEase(Ease.OutExpo))
            .Insert(secondSeqStartTime, transform.DOMove(targePoint + _dir, secondSeqDuringTime).SetEase(Ease.Linear));

        yield return new WaitForSeconds(Skill.SkillData.Duration);

        while (true)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, GameManager.Instance.player.transform.position, Time.deltaTime * Skill.SkillData.projectileSpeed * 4f);
            if (GameManager.Instance.player.transform.position == transform.position)
            {
                DestroyProjectile();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoIceSkill()
    {
        List<MonsterBase> target = ObjectManager.Instance.GetMonsterWithinCamera(1);
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > 3 || target == null)
            {
                DestroyProjectile();
                _timer = 0;
                break;
            }
            // 몬스터 풀링시스템 구축 후 체크
            // if (target[0].IsValid() == false)
            //     break;

            Vector2 direction = (Vector2)target[0].transform.position - _rigid.position;
            float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
            _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
            _rigid.velocity = transform.up * Skill.SkillData.projectileSpeed;

            yield return new WaitForFixedUpdate();
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

    IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(Skill.SkillData.Duration);
        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        SkillManager.Instance.DespawnProjectile(this);
    }


    void BounceProjectile(MonsterBase creature)
    {
        List<Transform> list = new List<Transform>();
        list = ObjectManager.Instance.GetFindMonstersInFanShape(creature.transform.position, _dir, 5.5f, 240);

        List<Transform> sortedList = (from t in list
                                      orderby Vector3.Distance(t.position, transform.position) descending
                                      select t).ToList();

        if (sortedList.Count == 0)
        {
            DestroyProjectile();
        }
        else
        {
            int index = Random.Range(sortedList.Count / 2, sortedList.Count);
            _dir = (sortedList[index].position - transform.position).normalized;
            _rigid.velocity = _dir * Skill.SkillData.BounceSpeed;
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterBase creature = collision.transform.GetComponent<MonsterBase>();
        // if (creature.IsValid() == false)
        //     return;

        if (this.IsValid() == false)
            return;

        switch (Skill.SkillType)
        {
            case SkillType.HolyProjectile:
            case SkillType.HolyPulse:
            case SkillType.IceSkill:
                _numPenerations--;
                if (_numPenerations < 0)
                {
                    _rigid.velocity = Vector3.zero;
                    DestroyProjectile();
                }
                break;
            // case SkillType.EnergyBolt:
            //     _bounceCount--;
            //     BounceProjectile(creature);
            //     if (_bounceCount < 0)
            //     {
            //         _rigid.velocity = Vector3.zero;
            //         DestroyProjectile();
            //     }
            //     break;
            case SkillType.FireSkill:
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
        // if (target.IsValid() == false)
        //     return;

        if (this.IsValid() == false)
            return;

        _enteredColliderList.Remove(target);

        if (_enteredColliderList.Count == 0 && _coDotDamage != null)
        {
            StopCoroutine(_coDotDamage);
            _coDotDamage = null;
        }
    }

}
