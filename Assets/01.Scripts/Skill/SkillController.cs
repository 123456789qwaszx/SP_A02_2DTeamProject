using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해결할 것
// 1. 오브젝트 풀링
// 2. 특정 범위 벗어나면 파괴
// 3. 풀링됐을 때, IsValid체크

public enum SkillType
{
    Projectile,
    //만들 것을 나중에 추가
}

public class SkillController : MonoBehaviour
{
    public SkillType SkillType { get; set; }

    Player _owner;
    Vector3 _moveDir;
    float _speed = 10.0f;
    float _lifeTime = 1.0f;
    int _damage;
    
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
            GameManager.Instance.DespawnProJectile(this);
        }
    }
    #endregion

    void Update()
    {
        transform.position += _moveDir * _speed * Time.deltaTime;
    }


    // 게임매니저의 Spawn함수에서 사용됨. Pooling 관련 처리.
    public void Init()
    {
        if (_isvalid)
        {
            _isvalid = false;
        }
        else
        {
            _isvalid = true;
        }
        StartDestroy(_lifeTime);
    }

    // 나중에 자동으로 스킬데이터 읽어오게 할 것
    public void SetInfo(Player owner, Vector2 moveDir, float lifeTime, int damage)
    {
        _lifeTime = lifeTime;
        _owner = owner;
        _moveDir = moveDir;
        _damage = damage;
    }

    // 몬스터와 충돌시 처리
    void OnTriggerEnter2D(Collider2D collision)
    {
        TestMonster monster = collision.gameObject.GetComponent<TestMonster>();
        // 몬스터 풀링 완성되면 그때 추가.
        // if (monster.isvalid == false)
        //     return;
        //
        if (this._isvalid == false)
            return;

        monster.OnDamaged(_owner, _damage);

        StopDestroy();

        GameManager.Instance.DespawnProJectile(this);
        _isvalid = false;
    }
}
