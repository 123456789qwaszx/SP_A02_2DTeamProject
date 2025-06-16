using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentslotUI : MonoBehaviour, IPointerClickHandler
{
    public List<ItemType> allowedItemTypes; // 이 슬롯에 장착 가능한 아이템 타입
    public Image iconImage;
    public Image rarityBorder;

    private GeneratedItem equippedItem;

    public void SetItem(GeneratedItem item)
    {
        equippedItem = item;
        iconImage.enabled = true;
        iconImage.sprite = ItemIconManager.Instance.GetIcon(item.itemType);
        rarityBorder.enabled = true;
        rarityBorder.color = GetRarityColor(item.rarity);
    }

    public bool CanEquip(ItemType type)
    {
        return allowedItemTypes.Contains(type);
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
        equippedItem = null;
        iconImage.enabled = false;
        rarityBorder.enabled = false;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (equippedItem == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var itemToUnequip = equippedItem; // 캐싱
            ClearSlot(); // 먼저 비움
            PlayerEquipmentManager.Instance.Unequip(itemToUnequip);
            InventoryManager.Instance.AddItem(itemToUnequip); // 여기만 1회 호출
            InventoryUIManager.instance.RefreshInventory(); // 정렬 갱신 추가
        }
    }
}
