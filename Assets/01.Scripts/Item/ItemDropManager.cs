using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    [System.Serializable]
    public class DropRate
    {
        public ItemRarity rarity;
        [Range(0f, 100f)] public float dropChance; // 확률 %
    }
    
    [Header("등급별 드랍 확률")]
    public List<DropRate> dropRates;
    
    [Header("드랍 가능한 아이템 풀")]
    public List<ItemType> dropitemTypes;
    
    public ItemGenerator itemGenerator;
    
    // 몬스터 사망시 호출될 아이템 드랍 시도 매서드 ItemDropManager.TryDropItem(transform.position)
    public void TryDropItem(Vector3 dropPosition)
    {
        ItemRarity selectedRarity = GetRandomRarity();
        ItemType selectedType = dropitemTypes[Random.Range(0, dropitemTypes.Count)];
        
        GeneratedItem newItem = itemGenerator.GenerateRandomItem(selectedRarity, selectedType);
        
        // UI/Player
        // 여기에 아이템 프리팹을 생성해서 World Space에 보여주는 부분"을 연결해야 함
        
        Debug.Log($"[드랍] {newItem.rarity} {newItem.itemType} 생성");
        
    }

    private ItemRarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rarity in dropRates) total += rarity.dropChance;
        
        float roll = Random.Range(0f, total);
        float current = 0f;
        foreach (var rate in dropRates)
        {
            current += rate.dropChance;
            if(roll <= current)
                return rate.rarity;
        }
        
        return ItemRarity.Normal;
    }
}
