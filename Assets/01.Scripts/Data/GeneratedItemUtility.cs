using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GeneratedItemUtility
{
    public static ItemData ToItemData(GeneratedItem item)
    {
        return new ItemData
        {
            itemName = item.itemName,
            rarity = item.rarity,
            itemType = item.itemType,
            buyPrice = item.buyPrice,
            sellPrice = item.sellPrice,
            usableClass = item.usableClass,
            options = item.options.Select(opt => new ItemOptionData(opt.optionType.ToString(), opt.value)).ToList()
        };
    }

    public static GeneratedItem ToGeneratedItem(ItemData data)
    {
        return new GeneratedItem
        {
            itemName = data.itemName,
            rarity = data.rarity,
            itemType = data.itemType,
            buyPrice = data.buyPrice,
            sellPrice = data.sellPrice,
            usableClass = data.usableClass,
            options = data.options.Select(opt => (Enum.Parse<ItemOptionType>(opt.optionName), opt.value)).ToList(),
            //icon = ItemIconManager.Instance.GetIcon(data.itemType)
        };
    }
}
