using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    [Header("Projectile")]
    public Transform indicator;
    public float _projectileCooldown = 0.3f;
    public float projectileLifeTime = 5f;
    public int projectileDamage = 10;


    void Start()
    {
        StartProjectile();
    }


    #region Projectile
    Coroutine _coProjectile;

    void StartProjectile()
    {
        if (_coProjectile != null)
            StopCoroutine(_coProjectile);

        _coProjectile = StartCoroutine(CoStartProjectile());
    }

    IEnumerator CoStartProjectile()
    {
        new WaitForSeconds(_projectileCooldown);

        while (true)
        {
            SkillController skill = GameManager.Instance.SpawnProjectile(transform.position);
            skill.SetInfo(GameManager.Instance.player, -(transform.position - indicator.position).normalized, projectileLifeTime, projectileDamage);
            yield return new WaitForSeconds(_projectileCooldown);
        }
    }
    #endregion
}
