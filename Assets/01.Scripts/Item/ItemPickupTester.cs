using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupTester : MonoBehaviour
{
    public float pickupRange = 2f; // 획득 범위

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (InventoryManager.Instance.IsFull())
            {
                Debug.Log("인벤토리가 가득 차 습득할 수 없습니다!");
                return;
            }
            
            TryPickupItem();
        }
        
    }

    private void TryPickupItem()
    {
        // 드랍된 아이템을 전부 찾기
        DroppedItemDisplay[] droppedItems = FindObjectsOfType<DroppedItemDisplay>();

        DroppedItemDisplay closest = null;
        float minDist = pickupRange;

        foreach (var item in droppedItems)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist <= minDist)
            {
                closest = item;
                minDist = dist;
            }
        }

        if (closest != null)
        {
            InventoryManager.Instance.AddItem(closest.GetItem());
            Destroy(closest.gameObject);
            Debug.Log("아이템 획득!");
        }
        else
        {
            Debug.Log("주변에 획득할 아이템이 없습니다.");
        }
    }
}
