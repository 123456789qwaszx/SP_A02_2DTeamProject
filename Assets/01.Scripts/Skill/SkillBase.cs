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

public class SkillBase : MonoBehaviour
{
    //임시
    public float CoolTime = 0.5f;
    bool _init = false;

    void Awake()
    {
        Init();
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

    [SerializeField]
    public SkillData _skillData;
    public SkillData SkillData
    {
        get
        {
            return _skillData;
        }
        set
        {
            _skillData = value;
        }
    }

    public bool IsLearnedSkill { get { return Level > 0; } }

    #region 동적 SkillData
    public int NumProjectiles; //회당 공격 횟수 그런데 레벨당으로 지정할거면 공격이긴함.
    public float ProjectileSpacing; // 투사체 간의 간격 (발사 갯수와 쿨타임 있는 스킬)
    #endregion


    public int TotalDamage { get; set; } = 0;
    // SetInfoSkillData 스크립터블 읽어와서 쏵 세팅해주기.
    public SkillData UpdateSkillData(int dataId = 0)
    {
        int id = 0;
        if (dataId == 0)
            id = Level < 2 ? (int)SkillType : (int)SkillType + Level - 1;
        else
            id = dataId;

        SkillData skillData = new SkillData();
        SkillManager.Instance._objects.TryGetValue($"{id}", out UnityEngine.Object obj);
        if (!skillData == obj)
            return SkillData;

        SkillData = skillData;

        return SkillData;


    }

    // 스킬 None에서 활성화 될때 실행
    public virtual void ActivateSkill()
    {
        UpdateSkillData();
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

    protected virtual void GenerateProjectile(PlayerController Owner, string prefabName, Vector3 startPos, Vector3 dir, Vector3 targetPos, SkillBase skill)
    {
        ProjectileController pc = SkillManager.Instance.SpawnProjectile(startPos, prefabName: prefabName);
        pc.SetInfo(Owner, startPos, dir, targetPos, skill);
    }

    public virtual void OnLevelUp()
    {
        if (Level == 0)
            ActivateSkill();
        Level++;
        UpdateSkillData();
    }
}