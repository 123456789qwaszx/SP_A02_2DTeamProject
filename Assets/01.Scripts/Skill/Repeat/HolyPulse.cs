using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyPulse : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.HolyPulse;
    }

    IEnumerator SetHolyPulse()
    {
        string prefabName = SkillData.PrefabLabel;

        if (GameManager.Instance.controller != null)
        {
            for (int i = 0; i < SkillData.projectileCount; i++)
            {
                Vector3 dir = Vector3.one;
                Vector3 startPos = GameManager.Instance.controller.transform.position;
                GenerateProjectile(GameManager.Instance.controller, prefabName, startPos, dir, Vector3.zero, this);

                yield return new WaitForSeconds(SkillData.ProjectileSpacing);
            }
        }
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetHolyPulse());
    }
}
