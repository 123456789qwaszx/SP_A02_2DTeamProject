using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyProjectile : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.HolyProjectile;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        DoSkillJob();
    }

    protected override void DoSkillJob()
    {
        // 아이템 데이터를 올바르게 바꿔야할 수도...
        string prefabName = SkillType.ToString();
        Vector3 position = GameManager.Instance.controller.transform.position;
        for (int i = 0; i < SkillData.projectileCount; i++)
        {
            Vector3 dir = -(position - indicator.transform.position).normalized;

            GenerateProjectile(GameManager.Instance.controller, prefabName, position, dir, indicator.transform.position, this);
        }
    }
}


// protected virtual void GenerateProjectile(PlayerController Owner, string prefabName, Vector3 startPos, Vector3 dir, Vector3 targetPos, GameObject skill_prefab)
// {
//     ProjectileController pc = SkillManager.Instance.SpawnProjectile(startPos, prefabName: prefabName);
//     pc.SetInfo(Owner, startPos, dir, targetPos, skill_prefab);
// }


// 처음 만들었던 것 참고용
// #region HolyProjectile
// Coroutine _coProjectile;

// public void StartProjectile()
// {
//     if (_coProjectile != null)
//         StopCoroutine(_coProjectile);

//     _coProjectile = StartCoroutine(CoStartProjectile());
// }

// IEnumerator CoStartProjectile()
// {
//     do
//     {
//         Vector2 dir = -(transform.position - indicator.position).normalized;

//         ProjectileController skill = SkillManager.Instance.SpawnHolyProjectile(transform.position);
//         skill.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);


//         skill.SetInfo(GameManager.Instance.controller, GameManager.Instance.controller.transform.position, -(transform.position - indicator.position).normalized, indicator.transform.position, SkillManager.Instance.holyProjectile_Prefab);
//         yield return new WaitForSeconds(skill._attackInterval);
//     }

//     while (true);
// }
// #endregion
