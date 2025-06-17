using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image rarityBorder;
    public Image itemIcon;

    private GeneratedItem item;
    public ShopBuyPopup shopBuyPopup;

    public void Set(GeneratedItem newItem)
    {
        item = newItem;
        var sprite = ItemIconManager.Instance.GetIcon(item.itemType);
        itemIcon.sprite = sprite;
        itemIcon.enabled = sprite != null;
        rarityBorder.color = GetRarityColor(item.rarity);
        rarityBorder.enabled = true;
    }
    
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
        {
            ItemTooltipUI.Instance.Show(item, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltipUI.Instance != null)
        {
            ItemTooltipUI.Instance.Hide();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;
        
        if (ShopUIManager.Instance.IsShopOpen)
        {
            if (item != null)
            {
                shopBuyPopup?.Open(item);
            }
            return;
        }
    }
}
