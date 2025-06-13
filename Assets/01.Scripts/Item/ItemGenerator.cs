using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public List<OptionPool> optionPools;

    private CharacterClass GetUsableClass(ItemType type)
    {
        return type switch
        {
            ItemType.Sword or ItemType.Axe => CharacterClass.Warrior,
            ItemType.Bow or ItemType.Crossbow => CharacterClass.Archer,
            ItemType.Staff or ItemType.Wand => CharacterClass.Mage,
            _ => CharacterClass.All
        };
    }

    public GeneratedItem GenerateRandomItem(ItemRarity rarity, ItemType itemType)
    {
        GeneratedItem item = new()
        {
            rarity = rarity,
            itemType = itemType,
            usableClass = GetUsableClass(itemType),
            itemName = $"{rarity} {itemType}"
        };

        var pool = optionPools.FirstOrDefault(p => p.itemType == itemType);
        if (pool == null)
        {
            Debug.LogError($"OptionPool for {itemType} not found.");
            return item;
        }

        int optionCount = rarity switch
        {
            ItemRarity.Normal => 2,
            ItemRarity.Magic => 3,
            ItemRarity.Rare => 4,
            ItemRarity.Unique => 4,
            ItemRarity.Set => 4,
            _ => 2
        };

        var selected = pool.possibleOptions
            .OrderBy(x => Random.value)
            .Take(optionCount);

        foreach (var opt in selected)
        {
            float val = opt.GetRandomValue(rarity);
            item.options.Add((opt.optionType, val));
        }

        return item;
    }

    public GeneratedItem GenerateUniqueItem(UniqueItemData data)
    {
        GeneratedItem item = new()
        {
            rarity = ItemRarity.Unique,
            itemType = data.itemType,
            usableClass = GetUsableClass(data.itemType),
            itemName = data.uniqueName
        };

        foreach (var opt in data.fixedOptions)
        {
            item.options.Add((opt.optionType, opt.GetValue()));
        }

        if (data.uniqueOption != null)
        {
            item.uniqueOption = (data.uniqueOption.optionName, data.uniqueOption.GetValue());
        }

        return item;
    }

    public GeneratedItem GenerateSetItem(SetItemData data, int equippedCount = 1)
    {
        var item = new GeneratedItem
        {
            rarity = ItemRarity.Set,
            itemType = data.itemType,
            usableClass = GetUsableClass(data.itemType),
            itemName = data.itemName
        };

        foreach (var opt in data.fixedOptions)
        {
            item.options.Add((opt.optionType, opt.GetValue()));
        }

        // 실제 발동 조건은 외부에서 처리, 여기선 전체 보너스를 다 저장해도 됨
        foreach (var bonus in data.setBonusOptions)
        {
            // item.setBonuses.Add((bonus.optionName, bonus.GetValue()));
            item.setBonuses.Add((bonus.optionName, bonus.value));
        }

        return item;
    }
}
