using System.Collections.Generic;
using Project.Enums;
[System.Serializable]
public class SaveData
{
    public List<PlayerData> players;        // 여러 플레이어의 데이터 저장
    // 그 외 저장할 전역 데이터들 추가 가능
    public int gold;
    public List<ItemData> inventoryItems = new();
    public Dictionary<CharacterClass, List<ItemData>> equippedItemsPerClass = new();
}