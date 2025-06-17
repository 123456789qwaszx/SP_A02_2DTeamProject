using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;
    public Image rarityBorder;

    private GeneratedItem item;
    
    public SellConfirmPopup sellPopup;

    public void SetItem(GeneratedItem newItem)
    {
        item = newItem;
        var sprite = ItemIconManager.Instance.GetIcon(item.itemType);
        iconImage.sprite = sprite;
        iconImage.enabled = sprite != null;
        rarityBorder.color = GetRarityColor(item.rarity);
        rarityBorder.enabled = true;
    }
    
    public void Clear()
    {
        item = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
        rarityBorder.enabled = false;
    }

    public bool IsEmpty => item == null;

    private Color GetRarityColor(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Normal => Color.gray,
            ItemRarity.Magic => Color.blue,
            ItemRarity.Rare => new Color(1f, 0.84f, 0f), // 금색
            ItemRarity.Unique => new Color(0.9f, 0.4f, 0f),
            ItemRarity.Set => Color.green,
            _ => Color.white
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && ItemTooltipUI.Instance != null)
            ItemTooltipUI.Instance.Show(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltipUI.Instance != null)
            ItemTooltipUI.Instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        // 상점이 열려 있으면 판매 처리
        if (ShopUIManager.Instance.IsShopOpen)
        {
            if (sellPopup != null)
            {
                sellPopup.Open(item);
            }
            return;
        }

        // 상점이 닫혀 있으면 장착 처리
        ItemEquipHandler.Instance.TryEquipItem(item);
    }
}
