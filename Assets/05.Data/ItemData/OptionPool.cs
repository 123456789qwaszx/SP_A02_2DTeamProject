using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Enums;

[CreateAssetMenu(menuName = "Item/Option Pool")]
public class OptionPool : ScriptableObject
{
    public ItemType itemType;
    public List<ItemOptionWithRarity> possibleOptions;
}
