using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public static ShopUIManager Instance;

    public GameObject shopPanel;
    public GameObject shopInventoryPanel;
    public ShopItemListUIManager listUIManager;

    void Awake() => Instance = this;

    public bool IsShopOpen => shopPanel != null && shopPanel.activeSelf;

    public void OpenShopUI()
    {
        //  인벤토리-장비 UI가 켜져 있다면 확실하게 닫기
        InventoryUIController controller = FindObjectOfType<InventoryUIController>();
        if (controller != null)
        {
            controller.inventoryPanel.SetActive(false);
            controller.equipmentPanel.SetActive(false);
            controller.tooltipPanel.SetActive(false);
        }
        if (ItemTooltipUI.Instance == null)
        {
            var tooltip = FindObjectOfType<ItemTooltipUI>(true); // 비활성 포함 검색
            tooltip.gameObject.SetActive(true);
            tooltip.Hide(); // 필요시 숨기기
        }

        shopPanel.SetActive(true);
        shopInventoryPanel.SetActive(true);
        
        var items = ShopManager.Instance.GetAvailableItems();
        listUIManager.ShowItems(items);

        ShopInventoryUIManager.instance?.RefreshInventory();
    }

    public void CloseShopUI()
    {
        shopPanel.SetActive(false);
        shopInventoryPanel.SetActive(false);

        // 팝업이나 툴팁도 같이 닫기
        ItemTooltipUI.Instance?.Hide();
    }
}
