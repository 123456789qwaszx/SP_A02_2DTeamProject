using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Player
{
    [Header("크리티컬 배수 증가량")]
    [SerializeField] private float critBonus_Normal;
    [SerializeField] private float critBonus_Special;
    [SerializeField] private float critBonus_Skill;

    [Header("크리티컬 데미지")]
    public float critDamage_Normal;
    public float critDamage_Special;
    public float critDamage_Skill;

    void Awake()
    {
        CharacterClass = CharacterClass.Wizard;
        // 기본 스탯
        str = 10;
        dex = 10;
        ints = 20;
        maxHP = 120; 
        hp = maxHP;  
        maxMP = 50;
        mp = maxMP;
        itemAP = 0.1f;

        moveSpeed = 0.8f + DEX * 0.04f;
        attackSpeed = 0.7f + DEX * 0.005f;

        // 전투 스탯
        attack = 10 + DEX * 0.2f;
        skillAttack = 20 + DEX * 0.3f;
        specialAttack = 20 + DEX * 0.4f;

        defense = 20;
        critical = 0.05f;

        // 회복
        hpRecovery = 5 + STR * 0.1f;
        mpRecovery = 5 + INT * 0.1f;

        // 크리티컬 데미지 증가 배수 (아이템 및 효과 증가량은 일단 0으로 가정)
        float itemBonus = 0f;
        float effectBonus = 0f;

        critBonus_Normal = 1f + (0.2f + itemBonus + effectBonus);
        critBonus_Special = 1f + (0.4f + itemBonus + effectBonus);
        critBonus_Skill = 1f + (0.3f + itemBonus + effectBonus);

        // 최종 크리티컬 데미지 계산
        critDamage_Normal = attack * critBonus_Normal;
        critDamage_Special = specialAttack * critBonus_Special;
        critDamage_Skill = skillAttack * critBonus_Skill;
    }

    /// <summary>
    /// 랜덤 값에 따라 일반 공격(Attack) 또는 크리티컬 공격(critDamage_Normal)의 데미지를 결정하여 반환
    /// </summary>
    /// <returns>최종 데미지 값</returns>
    public float GetDamage()
    {
        // 랜덤 값(Random.value, 0~1 사이)을 통해 크리티컬 여부 판정
        if (Random.value < critical)
        {
            return critDamage_Normal;
        }
        else
        {
            return Attack;
        }
    }

    public override PlayerData GetPlayerData()
    {
        PlayerData data = base.GetPlayerData();
        data.jobType = "Wizard";
        data.wizardData = GetWizardData();
        return data;
    }

    // Wizard용 데이터를 반환하는 메서드
    public WizardData GetWizardData()
    {
        WizardData data = new WizardData();

        BasePlayerData baseData = GetPlayerData().baseData;
        // Base 스탯을 복사
        data.CopyFrom(GetBasePlayerData());

        // Wizard 전용 스탯 할당
        data.critBonus_Normal = this.critBonus_Normal;
        data.critBonus_Special = this.critBonus_Special;
        data.critBonus_Skill = this.critBonus_Skill;
        data.critDamage_Normal = this.critDamage_Normal;
        data.critDamage_Special = this.critDamage_Special;
        data.critDamage_Skill = this.critDamage_Skill;

        return data;
    }
}
