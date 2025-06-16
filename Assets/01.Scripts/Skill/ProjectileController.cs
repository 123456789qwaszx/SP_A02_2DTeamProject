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
                break;
        }

        if (gameObject.activeInHierarchy)
            StartCoroutine(CoCheckDestory());

    }

    float _timer = 0;
    private float _rotateAmount = 1000;


    IEnumerator CoArrow()
    {

        Vector2 direction = (Vector2)GameManager.Instance.controller.transform.position +new Vector2(5,0) - _rigid.position;
        float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
        _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
        _rigid.velocity = transform.up * Skill.SkillData.projectileSpeed;

        yield return new WaitForFixedUpdate();

    }

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
                _numPenerations--;
                if (_numPenerations < 0)
                {
                    _rigid.velocity = Vector3.zero;
                    DestroyProjectile();
                }
                break;
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