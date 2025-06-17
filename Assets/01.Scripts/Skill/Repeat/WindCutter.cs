using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCutter : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.WindCutter;
    }

    protected override void DoSkillJob()
    {
        string prefabName = SkillType.ToString();
        if (GameManager.Instance.controller != null)
        {
            Vector3 startPos = GameManager.Instance.controller.transform.position;
            Vector3 dir = GameManager.Instance.MoveDir;
            for (int i = 0; i < SkillData.projectileCount; i++)
            {
                float angle = SkillData.AngleBetweenProj * (i - (SkillData.projectileCount - 1) / 2f);
                Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
                GenerateProjectile(GameManager.Instance.controller, prefabName, startPos, res.normalized, Vector3.zero, this);
            }
        }
    }
}