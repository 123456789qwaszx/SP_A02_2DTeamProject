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
    }

    protected override void DoSkillJob()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
    
}
