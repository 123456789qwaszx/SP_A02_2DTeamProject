using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillStat
{
    public SkillType SkillType;
    public int Level;
}

public abstract class SkillBase : MonoBehaviour
{
    //임시
    public float CoolTime = 0.5f;
    bool _init = false;

    void Awake()
    {
        Init();
    }

    Coroutine _coSkill;
    
    protected abstract void DoSkillJob();

    protected virtual IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(CoolTime);

        yield return wait;
        while (true)
        {
            DoSkillJob();
            yield return wait;
        }
    }


    public PlayerController Owner { get; set; }
    SkillType skillType;

    //스크립터블 오브젝트로 추가
    // 레벨에 따른 스탯값 모두 작성해두기. 
    public SkillType SkillType
    {
        get
        { return skillType; }
        set { skillType = value; }
    }

    int level = 0;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    //SkillData

    public float TotalDamage { get; set; } = 0;
    // SetInfoSkillData 스크립터블 읽어와서 쏵 세팅해주기.
    public void UpdateSkillData()
    {

    }

    // 스킬 None에서 활성화 될때 실행
    public virtual void ActivateSkill()
    {
        UpdateSkillData();
        
        if (_coSkill != null)
            StopCoroutine(_coSkill);

        gameObject.SetActive(true);
        _coSkill = StartCoroutine(CoStartSkill());
    }

    public virtual void OnLevel()
    {
        if (Level == 0)
            ActivateSkill();

        Level++;

        UpdateSkillData();
    }

    protected void HitEvent(Collider2D collision)
    {

    }

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    } 
}
/*
1. 레벨
2. 쿨타임
3. 캐스팅타임

*/