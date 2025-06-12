using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    [Header("Projectile")]
    public Transform _projectileSocket;
    public float _projectileCooldown = 0.3f;


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
        WaitForSeconds wait = new WaitForSeconds(_projectileCooldown);

        while (true)
        {
            SkillController skill = GameManager.Instance.SpawnProjectile(transform.position);
            skill.SetInfo(GameManager.Instance.player, GameManager.Instance.MoveDir);
            yield return wait;
        }
    }
    #endregion
}
