using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StageManager;

public class MonsterSpawnManager : Singleton<MonsterSpawnManager>
{
    [Header("몬스터 프리팹")]
    public GameObject[] regularPrefabs_1_5;
    public GameObject[] elitePrefabs_1_5;

    public GameObject[] regularPrefabs_6_10;
    public GameObject[] elitePrefabs_6_10;

    public GameObject[] regularPrefabs_11_16;
    public GameObject[] elitePrefabs_11_16;

    public GameObject[] midBossPrefabs;
    public GameObject[] bossPrefabs;
    public GameObject finalBossPrefab;

    [Header("스폰 설정")]
    public float spawnInterval = 2f;
    public float surroundInterval = 120f;
    public float minSpawnDistance = 7f;
    public float maxSpawnDistance = 12f;
    public int surroundMonstersCount = 20;

    [Header("스폰 타이밍 트리거")]
    private float[] eliteSpawnTimes = { 180f, 420f, 780f };
    private float[] midBossTimes = { 720f };

    private Transform player;
    private float playTime;
    private HashSet<float> triggeredTimes = new HashSet<float>();
    private bool isSpawning = true;
    private bool bossSpawned = false;

    private GameObject spawnedFinalBoss;
    private FinalBoss finalBossScript;

    private StageType currentStageType = StageType.Normal;


    void Start()
    {
        PlayerController controller = FindObjectOfType<PlayerController>();
        
        player = controller.transform;

        SetupStage(StageType.Normal);
    }

    void Update()
    {
        if (!isSpawning) return;

        playTime += Time.deltaTime;

        if (currentStageType == StageType.FinalBoss)
        {
            MonitorFinalBossPhase();
            return;
        }

        foreach (float timing in eliteSpawnTimes)
        {
            if (playTime >= timing && !triggeredTimes.Contains(timing))
            {
                SpawnFromPool(GetElitePrefabs());
                triggeredTimes.Add(timing);
            }
        }

        foreach (float timing in midBossTimes)
        {
            if (playTime >= timing && !triggeredTimes.Contains(timing))
            {
                SpawnFromPool(midBossPrefabs);
                triggeredTimes.Add(timing);
            }
        }

        if (currentStageType == StageType.Boss && playTime >= 900f && !bossSpawned)
        {
            if (GameManager.Instance.currentStage == 5)
                SpawnFromPool(bossPrefabs[0]);
            else if (GameManager.Instance.currentStage == 10)
                SpawnFromPool(bossPrefabs[1]);
            else if (GameManager.Instance.currentStage == 15)
                SpawnFromPool(bossPrefabs[2]);

            bossSpawned = true;
        }
    }

    IEnumerator SpawnRegularMonsters()
    {
        while (isSpawning)
        {
            Vector3 pos = GetValidSpawnPosition();
            GameObject[] prefabs = GetRegularPrefabs();
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject monster = PoolManager.Instance.Pop(prefab);
            
            MonsterBase monsterBase = monster.GetComponent<MonsterBase>();
            ObjectManager.Instance.Monsters.Add(monsterBase);
            InitPooledMonster(monster, pos);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SurroundSpawnRoutine()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(surroundInterval);
            if (playTime >= 900f) yield break;

            float radius = (minSpawnDistance + maxSpawnDistance) / 2f;
            GameObject[] prefabs = GetRegularPrefabs();
            GameObject prefab = prefabs[Random.Range(0, 6)];

            for (int i = 0; i < surroundMonstersCount; i++)
            {
                float angle = 360f * i / surroundMonstersCount;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
                Vector3 pos = player.position + dir * radius;

                if (IsOutsideCameraView(pos))
                {
                    GameObject monster = PoolManager.Instance.Pop(prefab);
                    InitPooledMonster(monster, pos);
                }
            }
        }
    }
    private GameObject[] GetRegularPrefabs()
    {
        int stage = GameManager.Instance.currentStage;

        if (stage >= 1 && stage <= 5)
            return regularPrefabs_1_5;
        else if (stage >= 6 && stage <= 10)
            return regularPrefabs_6_10;
        else
            return regularPrefabs_11_16;
    }

    private GameObject[] GetElitePrefabs()
    {
        int stage = GameManager.Instance.currentStage;

        if (stage >= 1 && stage <= 5)
            return elitePrefabs_1_5;
        else if (stage >= 6 && stage <= 10)
            return elitePrefabs_6_10;
        else
            return elitePrefabs_11_16;
    }

    void SpawnFromPool(GameObject[] prefabs)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject monster = PoolManager.Instance.Pop(prefab);
        InitPooledMonster(monster, GetValidSpawnPosition());
    }

    public void SpawnFromPool(GameObject prefab)
    {
        GameObject monster = PoolManager.Instance.Pop(prefab);
        InitPooledMonster(monster, GetValidSpawnPosition());
    }

    void InitPooledMonster(GameObject monster, Vector3 pos)
    {
        monster.transform.position = pos;
        monster.transform.rotation = Quaternion.identity;
        monster.SetActive(true);

        IMonster resettable = monster.GetComponent<IMonster>();
        if (resettable != null)
            resettable.ResetMonster();
    }

    Vector3 GetValidSpawnPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Vector3 pos = player.position + dir * distance;
            if (IsOutsideCameraView(pos)) return pos;
        }
        return player.position + Vector3.up * minSpawnDistance;
    }

    bool IsOutsideCameraView(Vector3 pos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(pos);
        return viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1;
    }

    public void StopSpawn()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    public void SetupStage(StageType type)
    {
        currentStageType = type;
        isSpawning = true;
        playTime = 0f;
        bossSpawned = false;
        triggeredTimes.Clear();

        switch (type)
        {
            case StageType.Normal:
            case StageType.Boss:
                StartCoroutine(SpawnRegularMonsters());
                StartCoroutine(SurroundSpawnRoutine());
                break;
            case StageType.FinalBoss:
                SpawnFinalBoss();
                break;
            case StageType.Event:
                isSpawning = false;
                break;
        }
    }

    void SpawnFinalBoss()
    {
        // 플레이어 위쪽에 스폰
        Vector3 spawnPos = player.position + Vector3.up * 10f;
        spawnedFinalBoss = Instantiate(finalBossPrefab, spawnPos, Quaternion.identity);
        finalBossScript = spawnedFinalBoss.GetComponent<FinalBoss>();
    }

    void MonitorFinalBossPhase()
    {
        if (spawnedFinalBoss == null || finalBossScript == null) return;

        float hpPercent = finalBossScript.HPPercent;

        // 2페이즈일때 중간보스도 추가로 10초마다 스폰
        if (hpPercent <= 0.3f && !triggeredTimes.Contains(0.3f))
        {
            finalBossScript.EnterPhase(3);
            StartCoroutine(FinalBossPhaseRoutine(phase: 3));
            triggeredTimes.Add(0.3f);
        }
        // 1페이즈일때 일반 몬스터, 엘리트 몬스터 10초마다 스폰
        else if (hpPercent <= 0.6f && !triggeredTimes.Contains(0.6f))
        {
            finalBossScript.EnterPhase(2);
            StartCoroutine(FinalBossPhaseRoutine(phase: 2));
            triggeredTimes.Add(0.6f);
        }
    }

    IEnumerator FinalBossPhaseRoutine(int phase)
    {
        while (spawnedFinalBoss != null)
        {
            switch (phase)
            {
                case 2:
                    SpawnFromPool(GetElitePrefabs());
                    SpawnFromPool(GetRegularPrefabs());
                    break;

                case 3:
                    SpawnFromPool(midBossPrefabs);
                    break;
            }

            yield return new WaitForSeconds(10f);
        }
    }
}
