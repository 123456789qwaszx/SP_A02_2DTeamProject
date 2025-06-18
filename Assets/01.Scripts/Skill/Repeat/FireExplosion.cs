using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 메테오 이펙트
public class FireExplosion : MonoBehaviour
{
    bool reserved;
    
    void Update()
    {
        if (gameObject.activeInHierarchy && reserved == false)
        StartCoroutine(CoCheckDestoryEffect(gameObject));
    }

    // 메테오 폭발 제거
    IEnumerator CoCheckDestoryEffect(GameObject go)
    {
        reserved = true;
        yield return new WaitForSeconds(3f);
        ResourceManager.Instance.Destroy(go);
    }
}