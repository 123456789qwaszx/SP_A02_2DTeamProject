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
    public ItemOptionType optionType;
    public List<RarityRange> rarityRanges;

    public float GetRandomValue(ItemRarity rarity)
    {
        var range = rarityRanges.FirstOrDefault(r => r.rarity == rarity);
        return range != null ? Random.Range(range.minValue, range.maxValue) : 0f;
    }
}

[System.Serializable]
public class FixedItemOption
{
    public ItemOptionType optionType;
    public float minValue;
    public float maxValue;
    

    public float GetValue()
    {
        return Random.Range(minValue, maxValue);
    }
}

[System.Serializable]
public class UniqueItemOption
{
    public string optionName;
    public float minValue;
    public float maxValue;
    

    public float GetValue()
    {
        return Random.Range(minValue, maxValue);
    }
}

[System.Serializable]
public class SetBonusOption
{
    [Tooltip("몇 개 착용 시 발동되는 보너스")]
    public int requiredCount;

    public string optionName;
    public float minValue;
    public float maxValue;

    public float GetValue() => UnityEngine.Random.Range(minValue, maxValue);
}