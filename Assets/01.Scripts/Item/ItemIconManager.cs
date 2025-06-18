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
        
        Debug.Log("[ItemIconManager] Awake 호출됨");
    }
    
    public Sprite GetIcon(ItemType type)
    {
        var v = visuals.FirstOrDefault(v => v.itemType == type);

        if (v == null)
        {
            Debug.LogWarning($"[ItemIconManager] ❌ 아이콘 매핑 없음: {type}");
            return null;
        }

        if (v.itemIcon == null)
        {
            Debug.LogWarning($"[ItemIconManager] ⚠️ 아이콘 스프라이트가 null: {type} (ItemVisualData는 있음)");
        }

        return v.itemIcon;
    }
}
