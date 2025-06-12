using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public List<OptionPool> optionPools;

    public GeneratedItem GenerateRandomItem(ItemRarity rarity, ItemType itemType)
    {
        GeneratedItem item = new();
        item.rarity = rarity;
        item.itemType = itemType;
        item.usableClass = GetUsableClass(itemType);
        item.itemName = $"{rarity} {itemType}";

        // 옵션 풀 찾기
        var pool = optionPools.FirstOrDefault(p => p.itemType == itemType);
        if (pool == null)
        {
            Debug.LogError("옵션 풀이 없습니다.");
            return item;
        }

        int baseOptionCount = rarity switch
        {
            ItemRarity.Normal => 2,
            ItemRarity.Magic => 3,
            ItemRarity.Rare => 4,
            ItemRarity.Unique => 4,
            ItemRarity.Set => 4,
            _ => 2
        };

        var baseOptions = pool.possibleOptions.OrderBy(x => UnityEngine.Random.value).Take(baseOptionCount);
        foreach (var opt in baseOptions)
            item.options.Add((opt.optionName, opt.GetRandomValue()));

        return item;
    }

    public GeneratedItem GenerateUniqueItem(UniqueItemData uniqueData)
    {
        GeneratedItem item = new();
        item.rarity = ItemRarity.Unique;
        item.itemType = uniqueData.itemType;
        item.usableClass = uniqueData.usableClass;
        item.itemName = uniqueData.uniqueName;

        foreach (var opt in uniqueData.fixedOptions)
            item.options.Add((opt.optionName, opt.GetRandomValue()));

        if (uniqueData.uniqueOption != null)
            item.uniqueOption = (uniqueData.uniqueOption.optionName, uniqueData.uniqueOption.GetRandomValue());

        return item;
    }

    public GeneratedItem GenerateSetItem(SetItemData setData)
    {
        GeneratedItem item = new();
        item.rarity = ItemRarity.Set;
        item.itemType = setData.itemType;
        item.usableClass = setData.usableClass;
        item.itemName = setData.itemName;

        foreach (var opt in setData.fixedOptions)
            item.options.Add((opt.optionName, opt.GetRandomValue()));

        foreach (var bonus in setData.setBonusOptions)
            item.setBonuses.Add((bonus.optionName, bonus.GetRandomValue()));

        return item;
    }

    private CharacterClass GetUsableClass(ItemType type)
    {
        return type switch
        {
            ItemType.Sword or ItemType.Axe => CharacterClass.Warrior,
            ItemType.Bow or ItemType.Crossbow => CharacterClass.Archer,
            ItemType.Staff or ItemType.Wand => CharacterClass.Mage,
            _ => CharacterClass.Warrior // 기본값은 전사
        };
    }
}
