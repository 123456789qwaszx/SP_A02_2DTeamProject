using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedItem
{
    public string id = System.Guid.NewGuid().ToString(); // 유일 ID
    public string itemName;
    public ItemRarity rarity;
    public ItemType itemType;
    public CharacterClass usableClass;

    public List<(ItemOptionType optionType, float value)> options = new();
    public (string optionName, float value)? uniqueOption;
    public List<(string optionName, float value)> setBonuses = new();
    public SetItemData setItemData;
}
