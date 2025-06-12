using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Unique Item")]
public class UniqueItemData : ScriptableObject
{
    public string uniqueName;
    public Sprite icon;
    public ItemType itemType;
    public CharacterClass usableClass;
    public List<ItemOptionWithRarity> fixedOptions; // 옵션 4줄
    public ItemOptionWithRarity uniqueOption; // 고유 옵션
}
