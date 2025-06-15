using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipHandler : MonoBehaviour
{
    // [SerializeField] private Player playerStats;
    // [SerializeField] private EquipmentslotUI[] equipmentSlots;
    //
    //
    // private void Awake()
    // {
    //     playerStats = FindObjectOfType<Player>();
    // }
    //
    // public bool TryEquipItem(GeneratedItem item)
    // {
    //     foreach (var slot in equipmentSlots)
    //     {
    //         if (slot.CanEquip(item))
    //         {
    //             // 직업 검사
    //             if (!IsUsableByClass(item))
    //             {
    //                 Debug.Log($"[{item.itemName}]은 {playerStats.characterClass}가 장착할 수 없습니다.");
    //                 return false;
    //             }
    //
    //             GeneratedItem previousItem = slot.GetEquippedItem();
    //
    //             if (previousItem != null)
    //             {
    //                 playerStats.RemoveOption(previousItem); // 기존 장비 옵션 제거
    //             }
    //
    //             slot.SetItem(item);
    //             playerStats.ApplyOption(item); // 새로운 장비 옵션 적용
    //
    //             Debug.Log($"{item.itemName} 장착 완료");
    //             return true;
    //         }
    //     }
    //
    //     Debug.LogWarning("장착 가능한 슬롯이 없습니다.");
    //     return false;
    // }
    //
    // private bool IsUsableByClass(GeneratedItem item)
    // {
    //     return item.usableClass == CharacterClass.None || item.usableClass == player.characterClass;
    // }
}
