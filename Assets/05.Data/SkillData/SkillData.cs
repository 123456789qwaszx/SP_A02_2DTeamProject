using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SkillDamage
{
    public SkillType type;
    public float value;
}

public enum SkillLevel
{
    _0,
    _1,
    _2,
    _3,
    _4,
    _5,
    _6
}

[Serializable]
public class SkillStatus
{
    public SkillLevel skillLevel;
    
    public int SkillIndex;

    public float DamageMultiplire = 5f; // 스킬 데미지
    public float AttackInterval = 1f; // 공격속도
    public float CoolTime = 1f; // 쿨타임

    public int projectileCount = 1; // 투사체 수
    public float BounceDist = 10f;
    public float ProjRange = 10f;
    public int NumBounce = 3; // 투사체 튀는 횟수

    public float ScaleMultiplier = 1; // 투사체 크기계수
    public float projectileSpeed = 5.0f; // 발사체 속도 배율


    public int NumPenerations = 0; // 관통 횟수

    public float BounceSpeed = 2; // 투사체 튕기는 속도
    public float AngleBetweenProj = 30f;// 투사체 사이 각도
    public float ProjectileSpacing; // 투사체 간의 거리


    public float Duration; // 스킬 지속 시간
    public float attackRangeMultiplier = 1.0f; // 공격 범위 배율
}

[CreateAssetMenu(fileName = "Skill", menuName = "New Skill")]

public class SkillData : ScriptableObject
{
    public string Description;
    public string PrefabLabel;

    public SkillType Type;
    public Sprite Icon;
    public SkillBase SkillPrefabs;

    [Header("SkillLevel")]
    public SkillStatus[] skillLevel;
    


}
