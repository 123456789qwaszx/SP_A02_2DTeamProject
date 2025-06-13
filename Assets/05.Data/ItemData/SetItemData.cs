using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Set Item")]
public class SetItemData : ScriptableObject
{
    public string setName;
    public string itemName;
    public Sprite setIcon;
    public ItemType itemType;
    public CharacterClass usableClass;
    public List<FixedItemOption> fixedOptions; // 옵션 4줄
    
    [Header("세트 효과 (개수별 보너스)")]
    public List<SetBonusOption> setBonusOptions;
}
