// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ItemDropManager : MonoBehaviour
// {
//     [System.Serializable]
//     public class DropRate
//     {
//         public ItemRarity rarity;
//         [Range(0f, 100f)] public float dropChance; // 확률 %
//     }
    
//     [Header("등급별 드랍 확률")]
//     public List<DropRate> dropRates;
    
//     [Header("드랍 가능한 아이템 풀")]
//     public List<ItemType> dropItemTypes;
    
//     [Header("유니크 아이템 목록")]
//     public List<UniqueItemData> uniqueItems;

//     [Header("세트 아이템 목록")]
//     public List<SetItemData> setItems;
    
//     public ItemGenerator itemGenerator;
//     public GameObject dropItemUIPrefab;         // 드랍된 UI 프리팹
//     public ItemIconManager iconManager;         // 아이콘 매니저 참조
    
//     private void Awake()
//     {
//         if (iconManager == null)
//         {
//             iconManager = ItemIconManager.Instance;
//             if (iconManager == null)
//             {
//                 Debug.LogError("❌ ItemIconManager 인스턴스를 찾을 수 없습니다!");
//             }
//             else
//             {
//                 Debug.Log("✅ ItemIconManager 자동 할당 완료");
//             }
//         }
//     }
    
//     // 몬스터 사망시 호출될 아이템 드랍 시도 매서드 ItemDropManager.TryDropItem(transform.position)
//     public void TryDropItem(Vector3 dropPosition, float finalDropChance)
//     {
//         float roll = Random.Range(0f, 100f);
//         if (roll > finalDropChance)
//             return;
        
//         ItemRarity rarity = GetRandomRarity();

//         GeneratedItem item;

//         if (rarity == ItemRarity.Unique && uniqueItems.Count > 0)
//         {
//             var data = uniqueItems[Random.Range(0, uniqueItems.Count)];
//             item = itemGenerator.GenerateUniqueItem(data);
//         }
//         else if (rarity == ItemRarity.Set && setItems.Count > 0)
//         {
//             var data = setItems[Random.Range(0, setItems.Count)];
//             item = itemGenerator.GenerateSetItem(data);
//         }
//         else
//         {
//             ItemType type = dropItemTypes[Random.Range(0, dropItemTypes.Count)];
//             item = itemGenerator.GenerateRandomItem(rarity, type);
//         }

//         Sprite icon = iconManager.GetIcon(item.itemType);

//         // 드랍 UI 생성 (드랍 상태로 표시)
//         Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(0.5f, 1.5f);
//         Vector3 dropPos = dropPosition + new Vector3(offset.x, offset.y, 0f);

//         GameObject ui = Instantiate(dropItemUIPrefab, dropPos + Vector3.up * 1.5f, Quaternion.identity);
//         ui.GetComponent<DroppedItemDisplay>().Setup(item, icon, ItemDisplayMode.Drop);
//     }

//     private ItemRarity GetRandomRarity()
//     {
//         float total = 0f;
//         foreach (var rarity in dropRates) total += rarity.dropChance;
        
//         float roll = Random.Range(0f, total);
//         float current = 0f;
//         foreach (var rate in dropRates)
//         {
//             current += rate.dropChance;
//             if(roll <= current)
//                 return rate.rarity;
//         }
        
//         return ItemRarity.Normal;
//     }
// }
