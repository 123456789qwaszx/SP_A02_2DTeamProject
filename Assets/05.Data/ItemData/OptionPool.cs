using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Option Pool")]
public class OptionPool : ScriptableObject
{
    ItemType itemType;
    public List<ItemOption> possibleOptions;
}
