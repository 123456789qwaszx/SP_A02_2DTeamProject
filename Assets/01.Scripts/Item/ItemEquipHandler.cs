using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipHandler : MonoBehaviour
{
    [SerializeField] private EquipmentslotUI[] equipmentSlots;

    /// <summary>
    /// 아이템을 해당 슬롯에 장착 시도
    /// </summary>
    public bool TryEquipItem(GeneratedItem item)
    {
        foreach (var slot in equipmentSlots)
        {
            if (slot.CanEquip(item))
            {
                // 기존 아이템 제거
                var previousItem = slot.GetEquippedItem();
                if (previousItem != null)
                {
                    // 추후 장착 해제 시 작업이 있다면 여기에
                    Debug.Log($"기존 아이템 {previousItem.itemName} 제거");
                }

                // 장착 처리
                slot.SetItem(item);

                Debug.Log($"아이템 {item.itemName} 장착 완료");
                return true;
            }
        }

        Debug.LogWarning("장착 가능한 슬롯이 없습니다.");
        return false;
    }
}
