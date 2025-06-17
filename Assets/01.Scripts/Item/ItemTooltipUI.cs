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

    public void Show(GeneratedItem item, Vector3 position, bool isShopItem = false)
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
            text.text = $"{opt.Item1}: +{opt.Item2:F2}";
        }

        // 유니크 고유 옵션
        if (item.rarity == ItemRarity.Unique && item.uniqueOption != null)
        {
            var go = Instantiate(optionTextPrefab, optionContainer);
            var text = go.GetComponent<TextMeshProUGUI>();
            text.color = new Color(0.9f, 0.4f, 0f);
            text.text = $"<i>고유 효과: {item.uniqueOption.Value.optionName} +{item.uniqueOption.Value.value:F2}</i>";
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
        
        // 구매/판매 가격 표시
        var priceGO = Instantiate(optionTextPrefab, optionContainer);
        var priceText = priceGO.GetComponent<TextMeshProUGUI>();

        if (isShopItem)
        {
            priceText.color = Color.yellow;
            priceText.text = $"<b>구매 가격:</b> {item.buyPrice:N0} G";
        }
        else
        {
            priceText.color = Color.cyan;
            priceText.text = $"<b>판매 가격:</b> {item.sellPrice:N0} G";
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(panel.GetComponent<RectTransform>());

        // 이제 보여주기
        panel.SetActive(true);
        
        // 한 프레임 뒤에 위치 조정
        StartCoroutine(SetPositionDelayed(position));
    }

    private IEnumerator SetPositionDelayed(Vector3 basePosition)
    {
        yield return null; // 한 프레임 대기: Layout이 완전히 적용된 이후

        Vector3 offset = new Vector3(260f, -210f, 0f);
        Vector3 pos = basePosition + offset;

        // 패널 크기 기반으로 화면 안에 맞게 위치 제한
        RectTransform rect = panel.GetComponent<RectTransform>();
        pos.x = Mathf.Min(pos.x, Screen.width - rect.rect.width);
        pos.y = Mathf.Max(pos.y, rect.rect.height);
        panel.transform.position = pos;
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
