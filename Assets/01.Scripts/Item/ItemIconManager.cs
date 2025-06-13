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
        return v != null ? v.itemIcon : null;
    }
}
