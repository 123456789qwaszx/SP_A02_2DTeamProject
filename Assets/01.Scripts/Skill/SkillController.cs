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

    GameObject _owner;
    Vector3 _moveDir;
    float _speed = 10.0f;
    float _lifeTime = 10.0f;


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

        if (IsValid)
        {
            PoolManager.Instance.Pop(gameObject);
        }
    }

    #endregion
    public bool Init()
    {
        StartDestroy(_lifeTime);

        return true;
    }

    public void SetInfo(int templateID, GameObject owner, Vector3 moveDir)
    {
        // 데이터 받아오기
        // 만약 데이터가 없다면

        _owner = owner;
        _moveDir = moveDir;
        // 템플릿 아이디를 토대로 스킬데이터를 받아오기
    }

    public void UpdateController()
    {
        transform.position += _moveDir * _speed * Time.deltaTime;
    }

    void OnTriggerEner2D(Collider2D collision)
    {
        
    }

}
