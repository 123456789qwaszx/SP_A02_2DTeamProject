using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    [Header("HolyProjectile")]
    public Transform indicator;
    public float _projectileCooldown = 0.3f;

    [Header("HolyPulse")]
    public Transform Swing;


    void Start()
    {
        StartProjectile();
        StartPulse();
    }


    #region HolyProjectile
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
            Vector2 dir = -(transform.position - indicator.position).normalized;

            SkillController skill = GameManager.Instance.SpawnHolyProjectile(transform.position);
            skill.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            skill.SetInfo(GameManager.Instance.player, -(transform.position - indicator.position).normalized, skill.lifeTime, skill.damage);
            yield return new WaitForSeconds(_projectileCooldown);
        }
    }
    #endregion

    #region HolyPulse
    Coroutine _coPulse;

    void StartPulse()
    {
        if (_coPulse != null)
            StopCoroutine(_coPulse);

            _coPulse = StartCoroutine(CoStartPulse());
    }

    IEnumerator CoStartPulse()
    {
        new WaitForSeconds(_projectileCooldown);

        while (true)
        {
            SkillController skill = GameManager.Instance.SpawnHolyPulse(transform.position);

            skill.SetInfo(GameManager.Instance.player, GameManager.Instance.MoveDir, skill.lifeTime = 2f, skill.damage);
            yield return new WaitForSeconds(_projectileCooldown);
        }
    }
    #endregion

}
