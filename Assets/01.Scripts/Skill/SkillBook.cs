using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    Coroutine _coProjectile;

    void Update()
    {
        StartProjectile();
    }

    void StartProjectile()
    {
        if (_coProjectile != null)
            StopCoroutine(_coProjectile);

        _coProjectile = StartCoroutine(CoStartProjectile());
    }

    IEnumerator CoStartProjectile()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            GameObject skill = GameManager.Instance.SpawnProjectile();
            skill.transform.position = this.transform.position;
            yield return wait;
        }
    }
}
