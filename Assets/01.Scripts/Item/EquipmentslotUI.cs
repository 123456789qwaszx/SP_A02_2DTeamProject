using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentslotUI : MonoBehaviour
{
    public List<ItemType> allowedItemTypes; // 이 슬롯에 장착 가능한 아이템 타입
    public Image iconImage;
    public Image rarityBorder;

    private GeneratedItem equippedItem;

    public void SetItem(GeneratedItem item)
    {
        equippedItem = item;

        if (item != null)
        {
            iconImage.sprite = ItemIconManager.Instance.GetIcon(item.itemType);
            iconImage.color = Color.white;

            rarityBorder.color = GetRarityColor(item.rarity);
            rarityBorder.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0); // 투명하게
            rarityBorder.enabled = false;
        }
    }

    public bool CanEquip(GeneratedItem item)
    {
        return allowedItemTypes.Contains(item.itemType);
    }

    private Color GetRarityColor(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Normal => Color.gray,
            ItemRarity.Magic => Color.blue,
            ItemRarity.Rare => new Color(1f, 0.84f, 0f),
            ItemRarity.Unique => new Color(0.9f, 0.4f, 0f),
            ItemRarity.Set => Color.green,
            _ => Color.white
        };
    }

    public GeneratedItem GetEquippedItem()
    {
        return equippedItem;
    }

    public void ClearSlot()
    {
        SetItem(null);
    }
}
