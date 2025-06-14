using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public Image rarityBorder;
    // public TextMeshProUGUI quantityText;

    private GeneratedItem item;

    public void SetItem(GeneratedItem newItem)
    {
        item = newItem;

        iconImage.sprite = ItemIconManager.Instance.GetIcon(item.itemType); // 아이콘 설정
        rarityBorder.color = GetRarityColor(item.rarity);           // 테두리 색상 설정
        // quantityText.text = ""; // 아직 스택 시스템 없으면 공백

        // 향후 클릭 시 상세 보기 추가 가능
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
}
