using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<GeneratedItem> items = new List<GeneratedItem>();
    
    [SerializeField] private int maxSlotCount = 30;
    public int MaxSlotCount => maxSlotCount;

    public List<GeneratedItem> Items => items;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 필요시
    }

    /// <summary>
    /// 인벤토리에 아이템 추가
    /// </summary>
    public void AddItem(GeneratedItem item)
    {
        items.Add(item);
        Debug.Log($"아이템 추가됨: {item.itemName}");
        InventoryUIManager.instance?.RefreshInventory(); // UI 갱신
    }

    /// <summary>
    /// 인벤토리에서 아이템 제거
    /// </summary>
    public void RemoveItem(GeneratedItem item)
    {
        items.Remove(item);
        InventoryUIManager.instance?.RefreshInventory();
    }

    public GeneratedItem GetItem(int index)
    {
        if (index < 0 || index >= items.Count) return null;
        return items[index];
    }

    public void ClearInventory()
    {
        items.Clear();
        InventoryUIManager.instance?.RefreshInventory();
    }
    
    public bool IsFull()
    {
        return items.Count >= maxSlotCount;
    }
}
