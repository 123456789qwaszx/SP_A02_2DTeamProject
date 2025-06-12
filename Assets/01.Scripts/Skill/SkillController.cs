using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillController : MonoBehaviour
{
    public SkillType skillType;

    Player _owner;
    Vector3 _moveDir;
    float _speed = 5.0f;
    public float lifeTime = 1.0f;
    public int damage=10;

    bool _isvalid = false;

    // 스킬 파괴함수. 모든 스킬이 공용으로 사용할 예정
    #region Destroy
    Coroutine _coDestroy;

    public void StartDestroy(float delaySeconds)
    {
        StopDestroy();
        _coDestroy = StartCoroutine(CoDestroy(delaySeconds));
    }

    public void StopDestroy()
    {
        if (_coDestroy != null)
        {
            StopCoroutine(_coDestroy);
            _coDestroy = null;
        }
    }

    IEnumerator CoDestroy(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        if (_isvalid)
        {
            _isvalid = false;
            switch (this.skillType)
            {
                case SkillType.HolyProjectile:
                    GameManager.Instance.DespawnHolyProJectile(this);
                    break;
                case SkillType.HolyImpact:
                    GameManager.Instance.DeSpawnHolyImpact(this);
                    break;
                case SkillType.HolyPulse:
                    GameManager.Instance.DeSpawnHolyPulse(this);
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    void Update()
    {
        transform.position += _moveDir * _speed * Time.deltaTime;
    }


    // 게임매니저의 Spawn함수에서 사용됨. Pooling 관련 처리.
    public virtual void Init()
    {
        if (_isvalid)
        {
            _isvalid = false;
        }
        else
        {
            _isvalid = true;
        }
        StartDestroy(lifeTime);
    }

    // 나중에 자동으로 스킬데이터 읽어오게 할 것
    public virtual void SetInfo(Player owner, Vector2 moveDir, float lifeTime, int damage)
    {
        this.lifeTime = lifeTime;
        _owner = owner;
        _moveDir = moveDir;
        this.damage = damage;
    }

    // 몬스터와 충돌시 처리
    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterBase monster = collision.gameObject.GetComponent<MonsterBase>();
        // 몬스터 풀링 완성되면 그때 추가.
        // if (monster.isvalid == false)
        //     return;
        //
        if (this._isvalid == false)
            return;


        monster.TakeDamage(damage);

        switch (this.skillType)
        {
            case SkillType.HolyProjectile:
                StopDestroy();
                GameManager.Instance.DespawnHolyProJectile(this);
                break;
            case SkillType.HolyImpact:
                StopDestroy();
                GameManager.Instance.DeSpawnHolyImpact(this);
                break;
            case SkillType.HolyPulse:
                break;
            default:
                break;
        }
        _isvalid = false;
    }
}