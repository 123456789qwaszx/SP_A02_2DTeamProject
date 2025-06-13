using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecorSet
{
    public GameObject[] prefabs; // 이 배열이 인스펙터에 보이게 됨
}

public class InfiniteTileManager : MonoBehaviour
{
    public int dungeonLevel = 0; // 현재 던전 레벨
    public GameObject[] tilePrefabsPerLevel; // 던전별 타일 프리팹 리스트

    [Header("장식 관련 설정")]
    public List<DecorSet> decorSetsPerLevel; // 각 레벨별 장식 프리팹 세트
    public float decorSpawnChance = 0.3f; // 장식 생성 확률 (30%)

    public float tileSize = 15f;
    private Dictionary<Vector2Int, GameObject> spawnedTiles = new();
    private Dictionary<Vector2Int, GameObject> spawnedDecorations = new();

    private Transform playerTf;
    private int spawnRadius = 2;

    private void Start()
    {
        playerTf = GameManager.Instance.player.transform;
        UpdateTilesAroundPlayer();
    }

    private void Update()
    {
        UpdateTilesAroundPlayer();
    }

    void UpdateTilesAroundPlayer()
    {
        Vector2Int playerTilePos = WorldToTilePos(playerTf.position);

        // 주변 타일 생성
        for (int x = -spawnRadius; x <= spawnRadius; x++)
        {
            for (int y = -spawnRadius; y <= spawnRadius; y++)
            {
                Vector2Int tilePos = new(playerTilePos.x + x, playerTilePos.y + y);
                if (!spawnedTiles.ContainsKey(tilePos))
                {
                    SpawnTile(tilePos);
                }
            }
        }

        // 너무 멀어진 타일 제거
        List<Vector2Int> tilesToRemove = new();

        foreach (var tileEntry in spawnedTiles)
        {
            Vector2Int tilePos = tileEntry.Key;
            Vector3 tileWorldPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);
            float distance = Vector3.Distance(playerTf.position, tileWorldPos);

            if (distance > tileSize * (spawnRadius + 1)) // 한 칸 여유
            {
                tilesToRemove.Add(tilePos);
            }
        }

        foreach (Vector2Int pos in tilesToRemove)
        {
            PoolManager.Instance.Push(spawnedTiles[pos]);
            spawnedTiles.Remove(pos);

            if (spawnedDecorations.ContainsKey(pos))
            {
                PoolManager.Instance.Push(spawnedDecorations[pos]);
                spawnedDecorations.Remove(pos);
            }
        }
    }

    void SpawnTile(Vector2Int tilePos)
    {
        GameObject tilePrefab = tilePrefabsPerLevel[dungeonLevel];
        GameObject tile = PoolManager.Instance.Pop(tilePrefab);

        Vector3 spawnPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);
        tile.transform.position = spawnPos;
        spawnedTiles[tilePos] = tile;

        TrySpawnDecoration(spawnPos, tilePos);
    }

    void TrySpawnDecoration(Vector3 tilePosition, Vector2Int tileKey)
    {
        if (decorSetsPerLevel == null || decorSetsPerLevel.Count <= dungeonLevel)
            return;

        if (spawnedDecorations.ContainsKey(tileKey)) return;
        if (Random.value > decorSpawnChance) return;

        GameObject[] decorList = decorSetsPerLevel[dungeonLevel].prefabs;
        if (decorList == null || decorList.Length == 0) return;

        GameObject decorPrefab = decorList[Random.Range(0, decorList.Length)];
        GameObject decor = PoolManager.Instance.Pop(decorPrefab);

        Vector3 randomOffset = new Vector3(
            Random.Range(-tileSize / 2f, tileSize / 2f),
            Random.Range(-tileSize / 2f, tileSize / 2f),
            0f
        );

        decor.transform.position = tilePosition + randomOffset;
        spawnedDecorations[tileKey] = decor;
    }

    Vector2Int WorldToTilePos(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / tileSize);
        int y = Mathf.FloorToInt(pos.y / tileSize);
        return new Vector2Int(x, y);
    }
}
