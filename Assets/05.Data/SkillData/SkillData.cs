using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    None,
    HolyProjectile,
    HolyImpact,
    HolyPulse,
    FireSkill,
    IceSkill
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
    public string PrefabLabel;
    public string Description;

    public SkillType Type;
    public Sprite Icon;
    public SkillBase SkillPrefabs;

    public float DamageMultiplire; // 스킬 데미지
    public float AttackInterval; // 공격속도
    public float CoolTime; // 쿨타임

    public int CastingEffect; // 스킬 발동 시 효과
    public float CastingEffectPercentage; // 스킬 발동 효과 확률

    public int projectileCount = 1; // 투사체 수
    public float ScaleMultiplier = 1; // 투사체 크기계수
    public float projectileSpeed = 1.0f; // 발사체 속도 배율

    public int NumPenerations; // 관통 횟수

    public int NumBounce; // 투사체 튀는 횟수
    public float BounceSpeed; // 투사체 튕기는 속도

    public float Duration; // 스킬 지속 시간
    public float attackRangeMultiplier = 1.0f; // 공격 범위 배율
}
