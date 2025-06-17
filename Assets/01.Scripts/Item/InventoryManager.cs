using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<GeneratedItem> items = new List<GeneratedItem>();
    
    [SerializeField] private int maxSlotCount = 30;
    public int MaxSlotCount => maxSlotCount;

    public List<GeneratedItem> Items => new List<GeneratedItem>(items); // 항상 복사본 제공

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
        Debug.Log($"[AddItem] 현재 아이템 수: {items.Count}");
        InventoryUIManager.instance?.RefreshInventory(); // UI 갱신
    }

    /// <summary>
    /// 인벤토리에서 아이템 제거
    /// </summary>
    public void RemoveItem(GeneratedItem item)
    {
        // items.Remove(item);
        items.RemoveAll(i => i.id == item.id); // 같은 ID를 가진 아이템만 제거
        // InventoryUIManager.instance?.RefreshInventory();
        InventoryUIManager.instance?.DelayedRefresh();
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
    
    /// <summary>
    /// 저장을 위한 아이템 데이터 반환
    /// </summary>
    public List<ItemData> GetSaveItems()
    {
        return items.Select(GeneratedItemUtility.ToItemData).ToList();
    }

    /// <summary>
    /// 저장된 데이터를 불러와 인벤토리 복원
    /// </summary>
    public void LoadFromSave(List<ItemData> itemDatas)
    {
        items = itemDatas.Select(GeneratedItemUtility.ToGeneratedItem).ToList();
        InventoryUIManager.instance?.RefreshInventory();
    }
}
