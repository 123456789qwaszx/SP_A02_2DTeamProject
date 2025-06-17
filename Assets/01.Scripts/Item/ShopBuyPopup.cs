using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopBuyPopup : MonoBehaviour
{
    public TextMeshProUGUI confirmText;
    private GeneratedItem currentItem;

    public void Open(GeneratedItem item)
    {
        Debug.Log($"[ShopBuyPopup] Open called with: {item.itemName}");
        currentItem = item;
        gameObject.SetActive(true);

        if (confirmText != null)
        {
            confirmText.text = $"<b>{item.itemName}</b>을(를)\n<b>{item.buyPrice:N0}G</b>에 구매하시겠습니까?";
        }
    }

    public void ConfirmPurchase()
    {
        if (currentItem == null) return;

        bool success = ShopManager.Instance.BuyItem(currentItem);
        if (success)
        {
            ShopItemListUIManager listUI = FindObjectOfType<ShopItemListUIManager>();
            listUI.ShowItems(ShopManager.Instance.GetAvailableItems());
            ClosePopup();
        }
    }

    public void ClosePopup()
    {
        currentItem = null;
        gameObject.SetActive(false);
    }
}
