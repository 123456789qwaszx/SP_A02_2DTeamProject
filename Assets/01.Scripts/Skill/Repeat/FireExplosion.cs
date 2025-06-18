using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 메테오 이펙트
public class FireExplosion : MonoBehaviour
{
    float _reserved = 0f;
    
    void Update()
    {
        if (gameObject.activeInHierarchy && _reserved !=0)
        StartCoroutine(CoCheckDestoryEffect(gameObject));
    }

    // 메테오 폭발 제거
    IEnumerator CoCheckDestoryEffect(GameObject go)
    {
        _reserved += 1;
        yield return new WaitForSeconds(3f);
        ResourceManager.Instance.Destroy(go);
        _reserved -= 1;
    }
}