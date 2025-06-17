using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyPulse : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.HolyPulse;
    }


    protected override void DoSkillJob()
    {
        string prefabName = SkillData.PrefabLabel;

        Vector3 startPos = GameManager.Instance.controller.transform.position;
        Vector3 dir = GameManager.Instance.MoveDir;

        for (int i = 0; i < SkillData.projectileCount; i++)
        {
            GenerateProjectile(GameManager.Instance.controller, prefabName, startPos, dir, Vector3.zero, this);
        }

    }
}
