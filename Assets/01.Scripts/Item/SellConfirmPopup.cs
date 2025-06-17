using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellConfirmPopup : MonoBehaviour
{
    private GeneratedItem currentItem;
    public TextMeshProUGUI confirmText;

    public void Open(GeneratedItem item)
    {
        currentItem = item;
        gameObject.SetActive(true);
        
        if (confirmText != null)
        {
            confirmText.text = $"<b>{item.itemName}</b>을(를)\n<b>{item.sellPrice:N0}G</b>에 판매하시겠습니까?";
        }
    }

    public void ConfirmSale()
    {
        if (currentItem == null) return;
        
        ShopManager.Instance.SellItem(currentItem);
        ClosePopup();
    }

    public void ClosePopup()
    {
        currentItem = null;
        gameObject.SetActive(false);
    }
}
