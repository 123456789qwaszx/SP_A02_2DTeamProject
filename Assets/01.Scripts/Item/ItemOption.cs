using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RarityRange
{
    public ItemRarity rarity;
    public float minValue;
    public float maxValue;
}

[System.Serializable]
public class ItemOptionWithRarity
{
    public string optionName;
    public List<RarityRange> rarityRanges;

    public float GetRandomValue(ItemRarity rarity)
    {
        var range = rarityRanges.FirstOrDefault(r => r.rarity == rarity);
        return range != null ? UnityEngine.Random.Range(range.minValue, range.maxValue) : 0f;
    }
}
