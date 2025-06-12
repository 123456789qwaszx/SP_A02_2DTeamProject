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
            _ => CharacterClass.Warrior
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
            item.options.Add((opt.optionName, val));
        }

        return item;
    }

    public GeneratedItem GenerateUniqueItem(UniqueItemData data)
    {
        GeneratedItem item = new()
        {
            rarity = ItemRarity.Unique,
            itemType = data.itemType,
            usableClass = data.usableClass,
            itemName = data.uniqueName
        };

        foreach (var opt in data.fixedOptions)
        {
            item.options.Add((opt.optionName, opt.GetRandomValue(ItemRarity.Unique)));
        }

        if (data.uniqueOption != null)
        {
            item.uniqueOption = (data.uniqueOption.optionName, data.uniqueOption.GetRandomValue(ItemRarity.Unique));
        }

        return item;
    }

    public GeneratedItem GenerateSetItem(SetItemData data)
    {
        GeneratedItem item = new()
        {
            rarity = ItemRarity.Set,
            itemType = data.itemType,
            usableClass = data.usableClass,
            itemName = data.itemName
        };

        foreach (var opt in data.fixedOptions)
        {
            item.options.Add((opt.optionName, opt.GetRandomValue(ItemRarity.Set)));
        }

        foreach (var bonus in data.setBonusOptions)
        {
            item.setBonuses.Add((bonus.optionName, bonus.GetRandomValue(ItemRarity.Set)));
        }

        return item;
    }
}
