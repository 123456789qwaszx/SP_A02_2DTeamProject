using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해결할 것
// 1. 오브젝트 풀링
// 2. 특정 범위 벗어나면 파괴
// 3. 풀링됐을 때, IsValid체크

public enum SkillType
{

}

public class SkillController : MonoBehaviour
{
    public SkillType SkillType { get; set; }
    public bool IsValid;

    Player _owner;
    Vector3 _moveDir;
    float _speed = 10.0f;
    float _lifeTime = 1.0f;

    Coroutine _coDestroy;

    #region Temp
    int Damage;

    #endregion


    #region Destroy
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

        if (IsValid)
        {
            PoolManager.Instance.Pop(gameObject);
        }
    }
    #endregion

    public void Start()
    {
        Init();
    }

    public void Update()
    {
        transform.position += _moveDir * _speed * Time.deltaTime;
    }


    public bool Init()
    {
        StartDestroy(_lifeTime);

        return true;
    }

    public void SetInfo(Player owner, Vector2 moveDir)
    {
        _owner = owner;
        _moveDir = moveDir;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        TestMonster monster = collision.gameObject.GetComponent<TestMonster>();
        Debug.Log("Monster 충돌");
        Debug.Log(monster);
        if (this.IsValid == false)
            return;

        monster.OnDamaged(_owner, Damage);

        StopDestroy();

        GameManager.Instance.DespawnProJectile(this);
    }
}
