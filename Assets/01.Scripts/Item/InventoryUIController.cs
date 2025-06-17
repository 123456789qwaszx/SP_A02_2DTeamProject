using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject tooltipPanel;

    private void Update()
    {
        // I 키: 인벤/장비 토글
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isOpen = !inventoryPanel.activeSelf;

            //  열려고 할 때, 상점이 열려 있으면 닫기
            if (isOpen && ShopUIManager.Instance != null && ShopUIManager.Instance.IsShopOpen)
            {
                ShopUIManager.Instance.CloseShopUI();
            }

            inventoryPanel.SetActive(isOpen);
            equipmentPanel.SetActive(isOpen);
            tooltipPanel.SetActive(isOpen);

            if (isOpen)
                InventoryUIManager.instance?.RefreshInventory();
        }

        // ESC 키: 닫기 우선순위
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 1. 상점 닫기
            if (ShopUIManager.Instance != null && ShopUIManager.Instance.IsShopOpen)
            {
                ShopUIManager.Instance.CloseShopUI();
                return;
            }

            // 2. 인벤토리 닫기
            if (inventoryPanel.activeSelf || equipmentPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
                equipmentPanel.SetActive(false);
                tooltipPanel.SetActive(false);
                return;
            }
        }
    }
}
