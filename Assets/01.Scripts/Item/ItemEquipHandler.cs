using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Project.Enums;

public class ItemEquipHandler : Singleton<ItemEquipHandler>
{
    [SerializeField] private EquipmentslotUI[] equipmentSlots;
    
    // public void InitializeSlots()
    // {
    //     equipmentSlots = GetComponentsInChildren<EquipmentslotUI>();
    // }
    
    /// <summary>
    /// 아이템을 해당 슬롯에 장착 시도
    /// </summary>
    public bool TryEquipItem(GeneratedItem item)
    {
        var player = GameManager.Instance.player;
        if (!CanEquipForClass(player.CharacterClass, item.itemType))
        {
            Debug.Log($"[{player.CharacterClass}]은 {item.itemType}을 장착할 수 없습니다.");
            return false;
        }
        
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
    
    private bool CanEquipForClass(CharacterClass characterClass, ItemType itemType)
    {
        var commonTypes = new HashSet<ItemType>
        {
            ItemType.Helmet, ItemType.Armor, ItemType.Belt, ItemType.Boots,
            ItemType.Gloves, ItemType.Cloak, ItemType.Necklace,
            ItemType.Ring1, ItemType.Ring2
        };

        if (commonTypes.Contains(itemType))
            return true;

        switch (characterClass)
        {
            case CharacterClass.Warrior:
                return itemType == ItemType.Sword || itemType == ItemType.Axe;

            case CharacterClass.Archer:
                return itemType == ItemType.Bow || itemType == ItemType.Crossbow;

            case CharacterClass.Wizard:
                return itemType == ItemType.Staff || itemType == ItemType.Wand;

            case CharacterClass.All:
                return true;

            default:
                return false;
        }
    }
    
    public void RefreshUI()
    {
        Debug.Log("▶ RefreshUI 실행됨");
        Debug.Log($" 슬롯 수: {equipmentSlots.Length}");
        var equippedItems = PlayerEquipmentManager.Instance.GetCurrentEquippedItems();
        Debug.Log($" 현재 클래스 장착 아이템 수: {equippedItems.Count}");

        foreach (var slot in equipmentSlots)
        {
            foreach (var kvp in equippedItems)
            {
                Debug.Log($"[비교] 슬롯: {slot.name} | allowed: {string.Join(",", slot.allowedItemTypes)} | itemType: {kvp.Key} | Equal: {slot.allowedItemTypes.Contains(kvp.Key)}");
            }

            var match = equippedItems.FirstOrDefault(kvp => slot.CanEquip(kvp.Key));
            if (!match.Equals(default(KeyValuePair<ItemType, GeneratedItem>)))
            {
                Debug.Log($"[SetItem] 슬롯 {slot.name}에 아이템 {match.Value.itemName} 장착");
                slot.SetItem(match.Value);
            }
            //else
            //{
            //    Debug.Log($"[ClearSlot] 슬롯 {slot.name} 클리어");
            //    slot.ClearSlot();
            //}
        }
        Debug.Log($" 슬롯 수: {equipmentSlots.Length}");
    }
}
