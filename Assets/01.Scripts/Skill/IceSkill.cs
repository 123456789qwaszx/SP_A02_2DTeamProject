using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.IceSkill;
    }

    IEnumerator SetIceSkill()
    {
        string prefabName = SkillData.PrefabLabel;

        if (GameManager.Instance.player != null)
        {
            for (int i = 0; i < NumProjectiles; i++)
            {
                Vector3 dir = Vector3.one;
                Vector3 startPos = GameManager.Instance.player.transform.position;

                yield return new WaitForSeconds(ProjectileSpacing);
            }
        }
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetIceSkill());
    }
}
