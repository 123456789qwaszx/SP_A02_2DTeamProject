using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemEquipHandler : MonoBehaviour
{
    public static ItemEquipHandler Instance { get; private set; }

    [SerializeField] private EquipmentslotUI[] equipmentSlots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
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
}
