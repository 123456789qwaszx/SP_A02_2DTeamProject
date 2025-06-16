using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public override bool Init()
    {
        base.Init();
        return true;
    }

    #region CoSkill
    Coroutine _coSkill;

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        if (_coSkill != null)
            StopCoroutine(_coSkill);

        gameObject.SetActive(true);
       _coSkill = StartCoroutine(CoStartSkill());
    }

    // 실제 각 Reapeat스킬들의 동작 로직(각자 이것만 바꿔주면됨)
    protected abstract void DoSkillJob();

    protected virtual IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f/*SkillData.CoolTime*/);
       
        yield return wait;
        while (true)
        {
            if(SkillData.CoolTime != 0)
            DoSkillJob();
            yield return wait;
        }
    }
    #endregion
}