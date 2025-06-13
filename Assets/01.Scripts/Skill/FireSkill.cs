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


    // IEnumerator GenerateFire()
    // {
    //     List<MonsterBase> targets = 
    // }


    protected override void DoSkillJob()
    {
        throw new System.NotImplementedException();
    }

}
