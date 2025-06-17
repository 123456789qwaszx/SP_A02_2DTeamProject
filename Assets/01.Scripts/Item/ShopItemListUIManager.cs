using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemListUIManager : MonoBehaviour
{
    public Transform slotParent;           // 슬롯들이 들어갈 부모 (Grid Layout Group)
    public GameObject slotPrefab;          // ShopItemSlot 프리팹
    private readonly List<GameObject> currentSlots = new();
    public ShopBuyPopup shopBuyPopupPrefab;
    private List<GeneratedItem> currentItems = new();

    public void ShowItems(List<GeneratedItem> items)
    {
        ClearSlots();
        currentItems = new List<GeneratedItem>(items);

        foreach (var item in items)
        {
            GameObject go = Instantiate(slotPrefab, slotParent);
            var ui = go.GetComponent<ShopItemSlotUI>();
            ui.Set(item);
            ui.shopBuyPopup = shopBuyPopupPrefab;
            currentSlots.Add(go);
        }
    }
    
    public void RemoveItem(GeneratedItem item)
    {
        currentItems.Remove(item);               // ✅ 리스트에서 제거
        ShowItems(currentItems);                 // ✅ UI 갱신
    }

    public void ClearSlots()
    {
        foreach (var go in currentSlots)
        {
            Destroy(go);
        }
        currentSlots.Clear();
    }
}
