using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemPriceCalculator
{
    // 실제 상점에서 팔릴 때 적용할 비율
    public const float SellMultiplier = 0.2f;

    // 기본 가격 책정
    public static int GetBasePrice(ItemType type, ItemRarity rarity)
    {
        int basePrice = GetBasePriceByType(type);
        float rarityMultiplier = GetRarityMultiplier(rarity);

        return Mathf.RoundToInt(basePrice * rarityMultiplier);
    }

    public static int GetBuyPrice(GeneratedItem item)
    {
        return GetBasePrice(item.itemType, item.rarity);
    }

    public static int GetSellPrice(GeneratedItem item)
    {
        int buyPrice = GetBuyPrice(item);
        return Mathf.RoundToInt(buyPrice * SellMultiplier);
    }

    private static int GetBasePriceByType(ItemType type)
    {
        return type switch
        {
            ItemType.Sword => 200,
            ItemType.Axe => 220,
            ItemType.Staff => 300,
            ItemType.Wand => 300,
            ItemType.Bow => 250,
            ItemType.Crossbow => 250,
            ItemType.Armor => 300,
            ItemType.Helmet => 180,
            ItemType.Gloves => 150,
            ItemType.Cloak => 250,
            ItemType.Boots => 150,
            ItemType.Belt => 250,
            ItemType.Ring1 => 500,
            ItemType.Ring2 => 500,
            ItemType.Necklace => 400,
            _ => 100
        };
    }

    private static float GetRarityMultiplier(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Normal => 1f,
            ItemRarity.Magic => 1.5f,
            ItemRarity.Rare => 2.5f,
            ItemRarity.Unique => 10f,
            ItemRarity.Set => 10f,
            _ => 1f
        };
    }
}
