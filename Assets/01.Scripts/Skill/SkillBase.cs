using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    None = 0,
    HolyProjectile = 10001,
    HolyPulse = 10011,
    DarkArrow = 10021,
    DirtySwing = 10031,
    DirtyHalo = 10041,
    DirtyWing = 10051,
    PoisionBomb = 10061,
    WindCutter = 10071,
    Waterfall = 10081,
    FireExplosion = 10091,
    FireSwing = 10101,
    BloodChain = 10111,
    Meteor = 10121
    //만들 것을 나중에 추가
}

public class SkillBase : MonoBehaviour
{
    bool _init = false;
    public bool IsLearnedSkill { get { return Level > 0; } }
    public GameObject indicator;

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

    public SkillData SkillData;


    #region 동적 SkillData
    public int NumProjectiles; //회당 공격 횟수 그런데 레벨당으로 지정할거면 공격이긴함.
    public float ProjectileSpacing; // 투사체 간의 간격 (발사 갯수와 쿨타임 있는 스킬)
    #endregion


    public int TotalDamage { get; set; } = 100;

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
        // 지금은 바로바로 ScriptableObject로 뽑아오고 있다보니 고정 값인데,
        // Level 같이 동적인 데이터를 보관하기 위한 별도의 static 공간이 필요함.
        
        // 만약 바로 수정할거면, 키에서 Type을 받은 다음. 그 타입의 데이터를 가져오면 됨.
        SkillData _skillData = SkillData;
        SkillManager.Instance._skillData.TryGetValue($"{id}", out SkillData obj);
        if (!_skillData == obj)
            return SkillData;
        SkillData = _skillData;

        OnChangedSkillData();

        return SkillData;
    }

    // 레벨업 시 실제로 스킬의 Data를 레벨에 맞게 변화시킴
    public virtual void OnChangedSkillData() { }

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