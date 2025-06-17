using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance { get; private set; }
    
    // 직업별 장착 정보
    private Dictionary<CharacterClass, Dictionary<ItemType, GeneratedItem>> equippedPerClass = new();
    private Dictionary<ItemType, GeneratedItem> currentEquipped => GetEquippedFor(player.CharacterClass);

    private Player player;
    
    public Dictionary<string, int> equippedSetCounts = new(); // 세트 이름별 장착 개수
    private Dictionary<string, int> appliedSetBonusCounts = new(); // 세트 보너스 적용 수

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>();
        
        foreach (CharacterClass cls in Enum.GetValues(typeof(CharacterClass)))
        {
            if (!equippedPerClass.ContainsKey(cls))
                equippedPerClass[cls] = new Dictionary<ItemType, GeneratedItem>();
        }
    }
    
    private Dictionary<ItemType, GeneratedItem> GetEquippedFor(CharacterClass cls)
    {
        if (!equippedPerClass.ContainsKey(cls))
            equippedPerClass[cls] = new Dictionary<ItemType, GeneratedItem>();
        return equippedPerClass[cls];
    }

    public void Equip(GeneratedItem item)
    {
        currentEquipped[item.itemType] = item;
        
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
        if (currentEquipped.ContainsKey(item.itemType))
            currentEquipped.Remove(item.itemType);
        
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
    
    public void UnequipAll()
    {
        var equippedItems = new List<GeneratedItem>(currentEquipped.Values);
        foreach (var item in equippedItems)
        {
            Unequip(item);
        }
    }
    
    public void SaveCurrentEquipment(CharacterClass cls)
    {
        equippedPerClass[cls] = new Dictionary<ItemType, GeneratedItem>(currentEquipped);
    }
    
    public void LoadEquipmentForClass(CharacterClass cls)
    {
        if (!equippedPerClass.ContainsKey(cls)) return;

        UnequipAll();

        foreach (var kvp in equippedPerClass[cls])
        {
            Equip(kvp.Value);
        }
    }
    
    public Dictionary<ItemType, GeneratedItem> GetCurrentEquippedItems()
    {
        return new Dictionary<ItemType, GeneratedItem>(currentEquipped);
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

            // === 여기서 보너스 적용 ===
            switch (bonus.optionName)
            {
                case "특수공격력":
                    player.ApplyOption(ItemOptionType.특수공격력증가, bonus.value);
                    break;

                case "최대HP증가":
                    player.ApplyOption(ItemOptionType.최대HP량증가, bonus.value);
                    break;

                case "부활1회추가":
                    // 추후 구현 예정
                    // player.EnableRebirthOnce();
                    break;
            }

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
                // === 여기서 보너스 해제 ===
                switch (bonus.optionName)
                {
                    case "특수공격력":
                        player.RemoveOption(ItemOptionType.특수공격력증가, bonus.value);
                        break;

                    case "최대HP증가":
                        player.RemoveOption(ItemOptionType.최대HP량증가, bonus.value);
                        break;

                    case "부활1회추가":
                        // 추후 구현 예정
                        // player.DisableRebirth();
                        break;
                }

                appliedSetBonusCounts[setName] = bonus.requiredCount - 1;
            }
        }
    }
    
    public Dictionary<CharacterClass, List<ItemData>> GetAllEquippedItems()
    {
        var result = new Dictionary<CharacterClass, List<ItemData>>();

        foreach (var pair in equippedPerClass)
        {
            List<ItemData> itemList = pair.Value.Values
                                          .Select(GeneratedItemUtility.ToItemData)
                                          .ToList();
            result[pair.Key] = itemList;
        }

        return result;
    }
    
    public void LoadAllEquippedItems(Dictionary<CharacterClass, List<ItemData>> data)
    {
        foreach (var pair in data)
        {
            var equippedDict = new Dictionary<ItemType, GeneratedItem>();

            foreach (var itemData in pair.Value)
            {
                var item = GeneratedItemUtility.ToGeneratedItem(itemData);
                equippedDict[item.itemType] = item;
            }

            equippedPerClass[pair.Key] = equippedDict;
        }
    }
    
    public void ApplyEquippedItemsToCurrentPlayer()
    {
        var currentClass = player.CharacterClass;

        if (!equippedPerClass.TryGetValue(currentClass, out var equippedItems))
            return;

        UnequipAll(); // 현재 장착된 아이템 초기화

        foreach (var kvp in equippedItems)
        {
            Equip(kvp.Value); // 메모리에 저장된 아이템을 다시 장착
        }
    }
    
    public GeneratedItem GetEquippedItem(CharacterClass characterClass, ItemType itemType)
    {
        if (equippedPerClass.TryGetValue(characterClass, out var items))
        {
            if (items.TryGetValue(itemType, out var item))
            {
                return item;
            }
        }
        return null;
    }

    private ItemOptionType ParseEnum(string name) => Enum.TryParse(name, out ItemOptionType t) ? t : ItemOptionType.공격력;
}
