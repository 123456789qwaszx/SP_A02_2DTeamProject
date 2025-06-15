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

    public void SetItem(GeneratedItem newItem)
    {
        item = newItem;

        iconImage.sprite = ItemIconManager.Instance.GetIcon(item.itemType); // 아이콘 설정
        iconImage.enabled = true;

        rarityBorder.color = GetRarityColor(item.rarity); // 테두리 색상 설정
        rarityBorder.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        iconImage.enabled = false;
        rarityBorder.enabled = false;
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
        if (item != null)
            ItemTooltipUI.Instance.Show(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            bool equipped = FindObjectOfType<ItemEquipHandler>()?.TryEquipItem(item) ?? false;

            if (equipped)
                ClearSlot(); // 장착 성공 시 슬롯 비움
        }
    }
}
