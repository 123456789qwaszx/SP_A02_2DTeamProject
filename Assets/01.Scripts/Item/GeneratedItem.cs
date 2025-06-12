using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedItem
{
    public string itemName;
    public ItemRarity rarity;
    public ItemType itemType;
    public CharacterClass usableClass;

    public List<(string optionName, float value)> options = new();
    public (string optionName, float value)? uniqueOption;
    public List<(string optionName, float value)> setBonuses = new();
}
