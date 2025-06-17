using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI Instance;

    [Header("UI 요소들")]
    public GameObject panel;
    public Image iconImage;
    public TextMeshProUGUI itemNameText;
    public Transform optionContainer;
    public GameObject optionTextPrefab; // 텍스트 프리팹

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Hide();
    }

    public void Show(GeneratedItem item, Vector3 position)
    {
        if (item == null || Instance == null || iconImage == null) return;
        
        panel.SetActive(true);
        Vector3 offset = new Vector3(260f, -210f, 0f);
        Vector3 pos = position + offset;

        // 화면 밖으로 안 나가게 제한
        pos.x = Mathf.Min(pos.x, Screen.width - panel.GetComponent<RectTransform>().rect.width);
        pos.y = Mathf.Max(pos.y, panel.GetComponent<RectTransform>().rect.height);
        panel.transform.position = pos;
        
        iconImage.sprite = ItemIconManager.Instance.GetIcon(item.itemType);
        itemNameText.text = item.itemName;
        itemNameText.color = GetRarityColor(item.rarity);

        // 기존 옵션 제거
        foreach (Transform child in optionContainer)
            Destroy(child.gameObject);

        // 일반 옵션
        foreach (var opt in item.options)
        {
            var go = Instantiate(optionTextPrefab, optionContainer);
            var text = go.GetComponent<TextMeshProUGUI>();
            text.text = $"{opt.Item1}: +{opt.Item2}";
        }

        // 유니크 고유 옵션
        if (item.rarity == ItemRarity.Unique && item.uniqueOption != null)
        {
            var go = Instantiate(optionTextPrefab, optionContainer);
            var text = go.GetComponent<TextMeshProUGUI>();
            text.color = new Color(0.9f, 0.4f, 0f);
            text.text = $"<i>고유 효과: {item.uniqueOption.Value.optionName} +{item.uniqueOption.Value.value}</i>";
        }

        // 세트 효과
        if (item.rarity == ItemRarity.Set && item.setItemData != null)
        {
            string setName = item.setItemData.setName;
            int equippedCount = 0;

            if (PlayerEquipmentManager.Instance != null)
            {
                PlayerEquipmentManager.Instance.equippedSetCounts.TryGetValue(setName, out equippedCount);
            }

            foreach (var bonus in item.setItemData.setBonusOptions)
            {
                var go = Instantiate(optionTextPrefab, optionContainer);
                var text = go.GetComponent<TextMeshProUGUI>();

                // 발동 여부에 따라 색상 지정
                if (equippedCount >= bonus.requiredCount)
                    text.color = Color.green;
                else
                    text.color = Color.gray;

                text.text = $"[세트 {bonus.requiredCount}개] {bonus.optionName} +{bonus.value}";
            }
        }
        
    }

    public void Hide()
    {
        panel.SetActive(false);
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
}
