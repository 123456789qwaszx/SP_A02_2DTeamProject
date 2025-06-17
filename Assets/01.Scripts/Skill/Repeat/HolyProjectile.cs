using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HolyProjectile : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.HolyProjectile;
    }

    protected override void DoSkillJob()
    {
        string prefabName = SkillType.ToString();
        
        Vector3 startPos = GameManager.Instance.controller.transform.position;
        Vector3 dir = GameManager.Instance.MoveDir;
        
        Vector3 position = GameManager.Instance.controller.transform.position;
        for (int i = 0; i < SkillData.projectileCount; i++)
        {
            GenerateProjectile(GameManager.Instance.controller, prefabName, position, dir, Vector3.zero, this);
        }
    }
}