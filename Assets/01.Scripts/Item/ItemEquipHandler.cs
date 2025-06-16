using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemEquipHandler : MonoBehaviour
{
    [SerializeField] private EquipmentslotUI[] equipmentSlots;

    /// <summary>
    /// 아이템을 해당 슬롯에 장착 시도
    /// </summary>
    public bool TryEquipItem(GeneratedItem item)
    {
        var slot = equipmentSlots.FirstOrDefault(s => s.CanEquip(item.itemType));
        if (slot == null) return false;

        var previouslyEquipped = slot.GetEquippedItem();
        if (previouslyEquipped != null)
        {
            InventoryManager.Instance.AddItem(previouslyEquipped);
            PlayerEquipmentManager.Instance.Unequip(previouslyEquipped);
        }

        slot.SetItem(item);
        PlayerEquipmentManager.Instance.Equip(item);
        InventoryManager.Instance.RemoveItem(item);

        InventoryUIManager.instance.RefreshInventory(); // 추가!

        return true;
    }
}
