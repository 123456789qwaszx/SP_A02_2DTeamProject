using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.FireSkill;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }

    protected override void DoSkillJob()
    {
        string PrefabName = SkillType.ToString();

        Vector3 startPos = GameManager.Instance.player.transform.position;
        Vector3 dir = GameManager.Instance.MoveDir;
        for (int i = 0; i < SkillData.projectileCount; i++)
        {
            float angle = SkillData.AngleBetweenProj * (i - (SkillData.projectileCount - 1) / 2f);
            Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
            Debug.Log("임시 생성중");
            GenerateProjectile(GameManager.Instance.controller, PrefabName, startPos, res.normalized, Vector3.zero);
        }
    }

}
