using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemDisplayMode { Drop, Full }

public class DroppedItemDisplay : MonoBehaviour
{
    private GeneratedItem item;
    
    public Image iconImage;
    public Image borderImage; // 등급 테두리용
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI optionText;

    public void Setup(GeneratedItem newItem, Sprite icon, ItemDisplayMode mode)
    {
        item = newItem;
        
        iconImage.sprite = icon;

        // 등급별 테두리 색상
        borderImage.color = item.rarity switch
        {
            ItemRarity.Normal => Color.gray,
            ItemRarity.Magic => Color.cyan,
            ItemRarity.Rare => Color.yellow,
            ItemRarity.Unique => new Color(1f, 0.5f, 0f),
            ItemRarity.Set => Color.green,
            _ => Color.white
        };

        // 표시 모드에 따라 다르게 표시
        if (mode == ItemDisplayMode.Drop)
        {
            nameText.gameObject.SetActive(false);
            optionText.gameObject.SetActive(false);
        }
        else // Full 정보 표시
        {
            nameText.gameObject.SetActive(true);
            optionText.gameObject.SetActive(true);

            nameText.text = item.itemName;
            optionText.text = string.Join("\n", item.options.Select(o => $"{o.optionType}: {o.value}"));

            if (item.uniqueOption.HasValue)
                optionText.text += $"\n<color=#FFA500>고유옵션: {item.uniqueOption.Value.optionName}</color>";

            foreach (var bonus in item.setBonuses)
                optionText.text += $"\n<color=green>세트옵션: {bonus.optionName}</color>";
        }
        
        Debug.Log($"[드랍] {item.itemName} ({item.rarity})");

        foreach (var opt in item.options)
        {
            Debug.Log($" - {opt.optionType}: {opt.value}");
        }
    }
    
    public GeneratedItem GetItem()
    {
        return item;
    }
}
