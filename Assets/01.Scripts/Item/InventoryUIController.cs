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
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isOpen = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isOpen);
            equipmentPanel.SetActive(isOpen);
            tooltipPanel.SetActive(isOpen);
            InventoryUIManager.instance.RefreshInventory();
        }
    }
}
