using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemListGenerator : MonoBehaviour
{
    public int itemCount = 5;
    public ItemGenerator itemGenerator; // 아이템 생성기 참조
    public List<GeneratedItem> currentItems = new();

    void Start()
    {
        GenerateNewShopItems(); // 메인씬 처음 들어오면 자동 생성
        ShopManager.Instance.SetShopItems(currentItems);
    }

    public void GenerateNewShopItems()
    {
        currentItems.Clear();

        for (int i = 0; i < itemCount; i++)
        {
            var rarity = GetRandomRarity();
            var type = GetRandomType();
            var item = itemGenerator.GenerateRandomItem(rarity, type);

            currentItems.Add(item);
        }
    }

    private ItemRarity GetRandomRarity()
    {
        int rand = Random.Range(0, 100);
        if (rand < 60) return ItemRarity.Normal;
        else if (rand < 85) return ItemRarity.Magic;
        else return ItemRarity.Rare;
    }

    private ItemType GetRandomType()
    {
        var values = System.Enum.GetValues(typeof(ItemType));
        return (ItemType)values.GetValue(Random.Range(0, values.Length));
    }
}
