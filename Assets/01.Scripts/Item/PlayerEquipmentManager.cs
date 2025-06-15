using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance { get; private set; }
    
    private Dictionary<ItemType, GeneratedItem> equipped = new();
    private IEquipable playerStats;
    private Player player;
    
    private Dictionary<string, int> equippedSetCounts = new(); // 세트 이름별 장착 개수
    private Dictionary<string, int> appliedSetBonusCounts = new(); // 세트 보너스 적용 수

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>();
    }

    public void Equip(GeneratedItem item)
    {
        // 일반 옵션 적용
        foreach (var opt in item.options)
        {
            player.ApplyOption(opt.optionType, opt.value);
        }

        // 유니크 고유 옵션 적용
        if (item.rarity == ItemRarity.Unique && item.uniqueOption.HasValue)
        {
            string name = item.uniqueOption.Value.Item1;
            float val = item.uniqueOption.Value.Item2;

            // 특수 고유 효과 조건 분기
            // if (name == "죽음 시 1회 부활")
            //     playerStats.EnableRebirthOnce();
            // else if (name == "적 처치 시 HP 회복")
            //     playerStats.SetKillHealAmount(val);
            // 그 외 텍스트 기반 처리 등 추가 가능
        }

        // 세트 효과 적용
        if (item.rarity == ItemRarity.Set && item.setItemData != null)
        {
            ApplySetBonuses(item.setItemData);
        }
    }

    public void Unequip(GeneratedItem item)
    {
        // 일반 옵션 제거
        foreach (var opt in item.options)
        {
            player.RemoveOption(opt.optionType, opt.value);
        }

        // 유니크 고유 옵션 해제 (옵션당 해제 로직 필요 시 추가)
        if (item.rarity == ItemRarity.Unique && item.uniqueOption.HasValue)
        {
            string name = item.uniqueOption.Value.Item1;

            // if (name == "죽음 시 1회 부활")
            //     playerStats.DisableRebirth();
            // else if (name == "적 처치 시 HP 회복")
            //     playerStats.SetKillHealAmount(0);
        }

        // 세트 효과 해제
        if (item.rarity == ItemRarity.Set && item.setItemData != null)
        {
            RemoveSetBonuses(item.setItemData);
        }
    }
    
    void ApplySetBonuses(SetItemData setItem)
    {
        string setName = setItem.setName;

        if (!equippedSetCounts.ContainsKey(setName))
            equippedSetCounts[setName] = 0;

        equippedSetCounts[setName]++;

        int count = equippedSetCounts[setName];
        int alreadyApplied = appliedSetBonusCounts.ContainsKey(setName) ? appliedSetBonusCounts[setName] : 0;

        foreach (var bonus in setItem.setBonusOptions)
        {
            if (bonus.requiredCount > count || bonus.requiredCount <= alreadyApplied)
                continue;

            // 텍스트 기반 세트 효과 처리
            // if (bonus.optionName == "죽음 시 1회 부활")
            //     playerStats.EnableRebirthOnce();
            // else if (bonus.optionName == "화염 면역")
            //     playerStats.AddResistance(DamageType.Fire, 100);

            appliedSetBonusCounts[setName] = bonus.requiredCount;
        }
    }
    
    void RemoveSetBonuses(SetItemData setItem)
    {
        string setName = setItem.setName;

        if (!equippedSetCounts.ContainsKey(setName))
            return;

        equippedSetCounts[setName]--;

        int count = equippedSetCounts[setName];
        int alreadyApplied = appliedSetBonusCounts.ContainsKey(setName) ? appliedSetBonusCounts[setName] : 0;

        foreach (var bonus in setItem.setBonusOptions)
        {
            if (bonus.requiredCount > alreadyApplied)
                continue;

            if (bonus.requiredCount > count)
            {
                // 해제 조건 만족 시 해제 로직 수행
                // if (bonus.optionName == "죽음 시 1회 부활")
                //     playerStats.DisableRebirth();
                // else if (bonus.optionName == "화염 면역")
                //     playerStats.RemoveResistance(DamageType.Fire);

                appliedSetBonusCounts[setName] = bonus.requiredCount - 1;
            }
        }
    }

    private ItemOptionType ParseEnum(string name) => Enum.TryParse(name, out ItemOptionType t) ? t : ItemOptionType.공격력;
}
