using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var dropped = other.GetComponent<DroppedItemDisplay>();
        if (dropped == null) return;

        if (InventoryManager.Instance.IsFull())
        {
            Debug.Log("인벤토리가 가득 차서 아이템을 주울 수 없습니다.");
            return;
        }

        InventoryManager.Instance.AddItem(dropped.GetItem());
        Destroy(dropped.gameObject);

        Debug.Log($"아이템 자동 획득: {dropped.GetItem().itemName}");
    }
}
