using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager instance { get; private set; }

    public Transform slotParent; // ItemSlotGrid (Content)
    public GameObject inventorySlotPrefab;
    // public int slotCount = 30; // 기본 슬롯 수

    private List<InventorySlotUI> slotUIs = new();

    private void Awake()
    {
        instance = this;
        CreateSlots();

        // // 고정된 슬롯 미리 생성
        // for (int i = 0; i < slotCount; i++)
        // {
        //     var slot = Instantiate(inventorySlotPrefab, slotParent);
        //     var slotUI = slot.GetComponent<InventorySlotUI>();
        //     slotUI.Clear();
        //     slotUIs.Add(slotUI);
        // }
        //
        // Debug.Log($"슬롯 {slotCount}개 생성됨");
    }

    public void RefreshInventory()
    {
        RefreshInventory(InventoryManager.Instance.Items);
    }

    public void RefreshInventory(List<GeneratedItem> itemList)
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            if (i < itemList.Count)
                slotUIs[i].SetItem(itemList[i]);
            else
                slotUIs[i].Clear();
        }
    }

    public void FilterItems(InventoryTabType tabType)
    {
        var allItems = InventoryManager.Instance.Items;
        List<GeneratedItem> filtered = new();

        switch (tabType)
        {
            case InventoryTabType.All:
                filtered = allItems;
                break;
            case InventoryTabType.Weapon:
                filtered = allItems.Where(i => i.itemType.ToString().Contains("Sword") || i.itemType.ToString().Contains("Axe") ||
                                                i.itemType.ToString().Contains("Bow") || i.itemType.ToString().Contains("Crossbow") ||
                                                i.itemType.ToString().Contains("Staff") || i.itemType.ToString().Contains("Wand")).ToList();
                break;
            case InventoryTabType.Armor:
                filtered = allItems.Where(i => i.itemType.ToString().Contains("Helmet") || i.itemType.ToString().Contains("Armor") ||
                                                i.itemType.ToString().Contains("Glove") || i.itemType.ToString().Contains("Boot") ||
                                                i.itemType.ToString().Contains("Belt") || i.itemType.ToString().Contains("Cloak")).ToList();
                break;
            case InventoryTabType.Accessory:
                filtered = allItems.Where(i => i.itemType.ToString().Contains("Ring") || i.itemType.ToString().Contains("Necklace")).ToList();
                break;
        }

        RefreshInventory(filtered);
    }
    
    private void CreateSlots()
    {
        int count = InventoryManager.Instance.MaxSlotCount;

        for (int i = 0; i < count; i++)
        {
            var slot = Instantiate(inventorySlotPrefab, slotParent);
            var slotUI = slot.GetComponent<InventorySlotUI>();
            slotUIs.Add(slotUI);
            
        }
        Debug.Log($"슬롯 {count} 생성됨");
    }
}
