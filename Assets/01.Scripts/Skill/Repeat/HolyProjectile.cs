using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyProjectile : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.HolyProjectile;
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