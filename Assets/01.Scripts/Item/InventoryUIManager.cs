using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public Transform slotParent; // ItemSlotGrid
    public GameObject inventorySlotPrefab;

    private List<InventorySlotUI> slotUIs = new();
    private List<GeneratedItem> allItems = new();

    private void Start()
    {
        // 샘플 데이터 (나중에 실제 인벤토리 연결 예정)
        // allItems = ItemDatabase.GetAllItems(); // 인벤토리 연결 전까진 임시

        PopulateInventory(allItems);
    }

    public void PopulateInventory(List<GeneratedItem> items)
    {
        ClearSlots();

        foreach (var item in items)
        {
            GameObject slotObj = Instantiate(inventorySlotPrefab, slotParent);
            var slot = slotObj.GetComponent<InventorySlotUI>();
            slot.SetItem(item);
            slotUIs.Add(slot);
        }
    }

    public void FilterItems(InventoryTabType type)
    {
        List<GeneratedItem> filtered = type switch
        {
            InventoryTabType.All => allItems,
            InventoryTabType.Weapon => allItems.FindAll(i => i.itemType == ItemType.Sword ||
                                                             i.itemType == ItemType.Axe ||
                                                             i.itemType == ItemType.Bow ||
                                                             i.itemType == ItemType.Crossbow ||
                                                             i.itemType == ItemType.Staff ||
                                                             i.itemType == ItemType.Wand),
            InventoryTabType.Armor => allItems.FindAll(i => i.itemType == ItemType.Helmet ||
                                                            i.itemType == ItemType.Armor ||
                                                            i.itemType == ItemType.Belt ||
                                                            i.itemType == ItemType.Boots ||
                                                            i.itemType == ItemType.Gloves ||
                                                            i.itemType == ItemType.Cloak),
            InventoryTabType.Accessory => allItems.FindAll(i => i.itemType == ItemType.Necklace || 
                                                                i.itemType == ItemType.Ring1 || 
                                                                i.itemType == ItemType.Ring2),
            _ => allItems
        };

        PopulateInventory(filtered);
    }

    private void ClearSlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
        slotUIs.Clear();
    }
}
