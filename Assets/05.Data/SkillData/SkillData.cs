using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    HolyProjectile,
    HolyImpact,
    HolyPulse,
    FireSkill
    //만들 것을 나중에 추가
}

[Serializable]
public class SkillDamage
{
    public SkillType type;
    public float value;
}

[CreateAssetMenu(fileName = "Skill", menuName = "New Skill")]

public class SkillData : ScriptableObject
{
    public int SkillDataID;
    public string Name;
    public string Description;

    public SkillType Type;
    public Sprite Icon;
    public SkillBase SkillPrefabs;

    public float DamageMultiplire; // 스킬 데미지
    public float AttackInterval; // 공격속도
    public string CoolTime; // 쿨타임

    public int CastingEffect; // 스킬 발동 시 효과
    public float CastingEffectPercentage; // 스킬 발동 효과 확률

    public float NumPenerations; // 관통 횟수
    public float ProjSpeed; // 발사체 이동속도

    public float Duration; // 스킬 지속 시간


}
