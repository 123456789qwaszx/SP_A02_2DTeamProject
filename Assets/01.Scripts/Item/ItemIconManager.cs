using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemIconManager : MonoBehaviour
{
    public List<ItemVisualData> visuals;
    public static ItemIconManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public Sprite GetIcon(ItemType type)
    {
        var v = visuals.FirstOrDefault(v => v.itemType == type);
        if (v == null || v.itemIcon == null)
        {
            Debug.LogWarning($"[ItemIconManager] 아이콘을 찾을 수 없음: {type}");
            return null;
        }
        return v != null ? v.itemIcon : null;
    }
}
