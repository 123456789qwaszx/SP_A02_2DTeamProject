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
        
        // dataId값의 앞 네자리로 스킬타입, 뒷자리로 스킬레벨을 구분
        if (dataId == 0)
            id = Level < 2 ? (int)SkillType : (int)SkillType + Level - 1;
        else
            id = dataId;
        
        SkillData _skillData = SkillData;
        SkillManager.Instance._skillData.TryGetValue($"{id}", out SkillData obj);
        // 만약 1레벨이라면 가장 기본타입.
        if (!_skillData == obj)
            return SkillData;

        // 아니라면 2~6레벨에 만즌 SkillData를 가지고 온뒤,
        // 플레이어 스탯이나 추가효과로 인한 버프를 적용 한다.

        // TODO<스킬데이터 최신화 공간>
        // 레벨별 스킬데이터

        // SupportSkill에 따른 가산치
        // 이건 기존 Data랑 똑같은 구조와 방식을 가지지만, 딱 여기서만 사용되게 만들면 되겠다.
        // 이런 느낌.
        // foreach(SupportSkillData support in Managers.Game.Player.Skills.SupportSkills)
        // 그 후에 만약 SkillType.ToString() == SupportSkillName.ToString()이라면,
        // SkillData.ProjectileRange += supportSill.ProjectileRange;

        // 플레이어 스탯에 따른 가산치

        // 아이템 효과에 따른 가산치

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