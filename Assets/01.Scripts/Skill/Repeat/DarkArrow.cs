using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkArrow : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.DarkArrow;
    }

    protected override void DoSkillJob()
    {
        string prefabName = SkillData.PrefabLabel;

        if (GameManager.Instance.controller != null)
        {
            List<MonsterBase> target = ObjectManager.Instance.GetNearestMonsters(SkillData.skillLevel[Level].projectileCount);
            if (target != null)
            {
                for (int i = 0; i < target.Count; i++)
                {
                    Vector3 dir = (target[i].transform.position - GameManager.Instance.controller.transform.position).normalized;
                    Vector3 startPos = GameManager.Instance.controller.transform.position;
                    GenerateProjectile(GameManager.Instance.controller, prefabName, startPos, dir, Vector3.zero, this, Level);
                    //yield return new WaitForSeconds(SkillData.ProjectileSpacing);
                }
            }
        }
    }
}