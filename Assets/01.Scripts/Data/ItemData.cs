using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Enums;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public ItemRarity rarity;
    public ItemType itemType;
    public int buyPrice;
    public int sellPrice;
    public CharacterClass usableClass;
    public List<ItemOptionData> options = new();
}
