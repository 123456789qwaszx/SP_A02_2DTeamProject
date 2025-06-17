using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private List<GeneratedItem> availableItems = new();
    
    void Awake() => Instance = this;

    public void SetShopItems(List<GeneratedItem> items)
    {
        availableItems = new List<GeneratedItem>(items); // 복사 저장
    }
    
    public List<GeneratedItem> GetAvailableItems()
    {
        return availableItems;
    }

    public void RemoveShopItem(GeneratedItem item)
    {
        availableItems.Remove(item);
    }
    
    public void SellItem(GeneratedItem item)
    {
        PlayerGoldManager.Instance.AddGold(item.sellPrice);
        InventoryManager.Instance.RemoveItem(item);
        if (ShopInventoryUIManager.instance != null)
            ShopInventoryUIManager.instance.RefreshInventory();

        Debug.Log($"아이템 판매: {item.itemName} / +{item.sellPrice}골드");
    }
    
    public bool BuyItem(GeneratedItem item)
    {
        if (PlayerGoldManager.Instance.currentGold < item.sellPrice)
        {
            Debug.Log("골드 부족!");
            return false;
        }

        PlayerGoldManager.Instance.SpendGold(item.sellPrice);
        InventoryManager.Instance.AddItem(item);
        ShopInventoryUIManager.instance?.RefreshInventory();
        
        RemoveShopItem(item);

        Debug.Log($"[구매 완료] {item.itemName} / -{item.sellPrice}G");
        return true;
    }
}
