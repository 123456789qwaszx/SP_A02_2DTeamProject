using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Enums;

[CreateAssetMenu(menuName = "Item/Visual Data")]
public class ItemVisualData : ScriptableObject
{
    public ItemType itemType;
    public Sprite itemIcon;
}
