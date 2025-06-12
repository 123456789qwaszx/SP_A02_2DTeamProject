using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Set Item")]
public class SetItemData : ScriptableObject
{
    public string setName;
    public string itemName;
    public ItemType itemType;
    public CharacterClass usableClass;
    public List<ItemOption> fixedOptions; // 옵션 4줄
    public List<ItemOption> setBonusOptions; // 세트 효과
}
