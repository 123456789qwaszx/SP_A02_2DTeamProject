using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryTabType
{
    All,
    Weapon,
    Armor,
    Accessory,
}

public class InventoryTabButton : MonoBehaviour
{
    public InventoryTabType tabType; // 전체, 무기, 방어구, 장신구
    public InventoryUIManager inventoryUI;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        inventoryUI.FilterItems(tabType);
    }
}
