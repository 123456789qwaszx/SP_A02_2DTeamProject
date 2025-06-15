using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    None = 0,
    HolyProjectile = 10001,
    HolyImpact = 10011,
    HolyPulse = 10021,
    FireSkill = 10031,
    IceSkill = 10041
    //만들 것을 나중에 추가
}

public class SkillBase : MonoBehaviour
{
    bool _init = false;
    public bool IsLearnedSkill { get { return Level > 0; } }

    SkillType skillType;
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

    SkillData _skillData;
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


    #region 동적 SkillData
    public int NumProjectiles; //회당 공격 횟수 그런데 레벨당으로 지정할거면 공격이긴함.
    public float ProjectileSpacing; // 투사체 간의 간격 (발사 갯수와 쿨타임 있는 스킬)
    #endregion


    public int TotalDamage { get; set; } = 0;

    
    void Awake()
    {
        Init();
    }

    public SkillData UpdateSkillData(int dataId = 0)
    {
        int id = 0;
        if (dataId == 0)
            id = Level < 2 ? (int)SkillType : (int)SkillType + Level - 1;
        else
            id = dataId;
        // 이부분 수정할것
        Debug.Log("수정필요");
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

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    protected virtual void GenerateProjectile(PlayerController Owner, string prefabName, Vector3 startPos, Vector3 dir, Vector3 targetPos, GameObject skill_prefab)
    {
        ProjectileController pc = SkillManager.Instance.SpawnProjectile(startPos, prefabName: prefabName);
        pc.SetInfo(Owner, startPos, dir, targetPos, skill_prefab);
    }

    public virtual void OnLevelUp()
    {
        if (Level == 0)
            ActivateSkill();
        Level++;
        UpdateSkillData();
    }
}