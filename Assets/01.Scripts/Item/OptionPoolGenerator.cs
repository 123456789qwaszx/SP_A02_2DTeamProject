using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class OptionPoolGenerator
{
    [MenuItem("Tools/Generate OptionPools from CSV")]
    public static void GenerateOptionPoolsFromCSV()
    {
        string csvPath = "Assets/05.Data/ItemData/Final_OptionPool.csv";
        string saveFolder = "Assets/05.Data/OptionPool";

        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + csvPath);
            return;
        }

        var lines = File.ReadAllLines(csvPath).Skip(1);
        var optionPools = new Dictionary<ItemType, OptionPool>();

        foreach (string line in lines)
        {
            var tokens = line.Split(',');

            if (tokens.Length < 5)
                continue;

            if (!System.Enum.TryParse(tokens[0], out ItemType itemType)) continue;
            if (!System.Enum.TryParse(tokens[1], out ItemOptionType optionType)) continue;
            if (!System.Enum.TryParse(tokens[2], out ItemRarity rarity)) continue;

            float minValue = float.Parse(tokens[3]);
            float maxValue = float.Parse(tokens[4]);

            if (!optionPools.TryGetValue(itemType, out var pool))
            {
                pool = ScriptableObject.CreateInstance<OptionPool>();
                pool.itemType = itemType;
                pool.possibleOptions = new List<ItemOptionWithRarity>();
                optionPools[itemType] = pool;
            }

            var existing = pool.possibleOptions.FirstOrDefault(o => o.optionType == optionType);
            if (existing == null)
            {
                existing = new ItemOptionWithRarity
                {
                    optionType = optionType,
                    rarityRanges = new List<RarityRange>()
                };
                pool.possibleOptions.Add(existing);
            }

            existing.rarityRanges.Add(new RarityRange
            {
                rarity = rarity,
                minValue = minValue,
                maxValue = maxValue
            });
        }

        // Save all OptionPools
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        foreach (var kvp in optionPools)
        {
            string assetPath = $"{saveFolder}/{kvp.Key}_OptionPool.asset";
            AssetDatabase.CreateAsset(kvp.Value, assetPath);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("모든 OptionPool.asset 생성 완료!");
    }
}
