using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StageManager;

public class MonsterSpawnManager : Singleton<MonsterSpawnManager>
{
    [Header("\u2022 몬스터 프리팹")]
    public GameObject[] regularPrefabs_1_5;
    public GameObject[] elitePrefabs_1_5;

    public GameObject[] regularPrefabs_6_10;
    public GameObject[] elitePrefabs_6_10;

    public GameObject[] regularPrefabs_11_16;
    public GameObject[] elitePrefabs_11_16;

    public GameObject[] midBossPrefabs;
    public GameObject[] bossPrefabs;
    public GameObject finalBossPrefab;

    [Header("\u2022 스폰 설정")]
    public float spawnInterval = 2f;
    public float surroundInterval = 120f;
    public float minSpawnDistance = 7f;
    public float maxSpawnDistance = 12f;
    public int surroundMonstersCount = 20;

    [Header("\u2022 스폰 타이밍 트리거")]
    private float[] eliteSpawnTimes = { 180f, 420f, 780f };
    private float[] midBossTimes = { 720f };

    private Transform player;
    private float playTime;
    private HashSet<float> triggeredTimes = new HashSet<float>();
    private bool isSpawning = true;
    private bool bossSpawned = false;

    private GameObject spawnedFinalBoss;
    private FinalBoss finalBossScript;
    private float nextSurroundWarning;
    private bool midBossWarned = false;
    private bool countdownStarted = false;

    private StageType currentStageType = StageType.Normal;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.Instance.player != null)
            player = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" || !isSpawning)
            return;

        playTime += Time.deltaTime;

        // 경고 메시지 처리
        if (!midBossWarned && playTime >= 720f)
        {
            UIManager.Instance.ShowWarning("아주 강력한 몬스터가 다가옵니다!", 3f);
            midBossWarned = true;
        }

        if (playTime >= nextSurroundWarning && playTime < 900f)
        {
            UIManager.Instance.ShowWarning("곧 몬스터가 몰려옵니다!");
            nextSurroundWarning += surroundInterval;
        }

        if (currentStageType == StageType.Boss && !countdownStarted && playTime >= 890f)
        {
            countdownStarted = true;
            StartCoroutine(BossCountdownRoutine());
        }

        if (currentStageType == StageType.FinalBoss)
        {
            MonitorFinalBossPhase();
            return;
        }

        // 몬스터 스폰 트리거들
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
                int stage = GameManager.Instance.currentStage;
                int spawnCount = stage <= 5 ? 1 : stage <= 10 ? 2 : 3;

                List<int> selectedIndex = new List<int>();
                while (selectedIndex.Count < spawnCount)
                {
                    int randomIndex = Random.Range(0, midBossPrefabs.Length);
                    if (!selectedIndex.Contains(randomIndex))
                        selectedIndex.Add(randomIndex);
                }

                foreach (int index in selectedIndex)
                    SpawnFromPool(midBossPrefabs[index]);

                triggeredTimes.Add(timing);
            }
        }

        if (currentStageType == StageType.Boss && playTime >= 900f && !bossSpawned)
        {
            int stage = GameManager.Instance.currentStage;
            int bossIndex = stage == 5 ? 0 : stage == 10 ? 1 : 2;
            SpawnFromPool(bossPrefabs[bossIndex]);
            bossSpawned = true;
        }
    }

    IEnumerator BossCountdownRoutine()
    {
        for (int i = 10; i >= 1; i--)
        {
            UIManager.Instance.ShowWarning($"..{i}", 1f);
            yield return new WaitForSeconds(1f);
        }
        UIManager.Instance.ShowWarning("보스 출현!", 2f);
    }

    IEnumerator SpawnRegularMonsters()
    {
        while (isSpawning)
        {
            GameObject[] prefabs = GetRegularPrefabs();

            for (int i = 0; i < 3; i++)
            {
                Vector3 pos = GetValidSpawnPosition();
                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                GameObject monster = PoolManager.Instance.Pop(prefab);

                MonsterBase monsterBase = monster.GetComponent<MonsterBase>();
                ObjectManager.Instance.Monsters.Add(monsterBase);
                InitPooledMonster(monster, pos);
            }

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
        if (stage <= 5) return regularPrefabs_1_5;
        if (stage <= 10) return regularPrefabs_6_10;
        return regularPrefabs_11_16;
    }

    private GameObject[] GetElitePrefabs()
    {
        int stage = GameManager.Instance.currentStage;
        if (stage <= 5) return elitePrefabs_1_5;
        if (stage <= 10) return elitePrefabs_6_10;
        return elitePrefabs_11_16;
    }

    void SpawnFromPool(GameObject[] prefabs)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        SpawnFromPool(prefab);
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
        resettable?.ResetMonster();
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
        if (type == StageType.None)
        {
            isSpawning = false;
            StopAllCoroutines();
            return;
        }

        if (GameManager.Instance.player != null)
            player = GameManager.Instance.player.transform;

        currentStageType = type;
        isSpawning = true;
        playTime = 0f;
        nextSurroundWarning = surroundInterval - 10f;
        bossSpawned = false;
        midBossWarned = false;
        countdownStarted = false;
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
        }
    }

    void SpawnFinalBoss()
    {
        Vector3 spawnPos = player.position + Vector3.up * 10f;
        spawnedFinalBoss = Instantiate(finalBossPrefab, spawnPos, Quaternion.identity);
        finalBossScript = spawnedFinalBoss.GetComponent<FinalBoss>();
    }

    void MonitorFinalBossPhase()
    {
        if (spawnedFinalBoss == null || finalBossScript == null) return;

        float hpPercent = finalBossScript.HPPercent;

        if (hpPercent <= 0.3f && !triggeredTimes.Contains(0.3f))
        {
            finalBossScript.EnterPhase(3);
            StartCoroutine(FinalBossPhaseRoutine(3));
            triggeredTimes.Add(0.3f);
        }
        else if (hpPercent <= 0.6f && !triggeredTimes.Contains(0.6f))
        {
            finalBossScript.EnterPhase(2);
            StartCoroutine(FinalBossPhaseRoutine(2));
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
