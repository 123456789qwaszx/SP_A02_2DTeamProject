using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [Header("몬스터 프리팹")]
    public GameObject[] regularMonsterPrefabs;
    public GameObject[] eliteMonsterPrefabs;
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
    private bool isBossStage = false;

    private Transform player;
    private float playTime;
    private HashSet<float> triggeredTimes = new HashSet<float>();
    private bool isSpawning = true;
    private bool bossSpawned = false;

    private GameObject spawnedFinalBoss;
    private MonsterFinalBoss finalBossScript;
    private bool isFinalBossStage;

    void Start()
    {
        // 나중에 GameManger.Instance로 플레이어를 가져오도록 변경
        player = FindObjectOfType<PlayerController>().transform;

        if (isFinalBossStage)
        {
            //SpawnFinalBoss();   // 최종 보스 스테이지
        }
        else
        {
            StartCoroutine(SpawnRegularMonsters()); // 일반 몬스터 지속 스폰
            StartCoroutine(SurroundSpawnRoutine()); // 원형진 몬스터 스폰
        }
    }

    void Update()
    {
        playTime += Time.deltaTime;
        if (!isSpawning) return;

        // 엘리트 몬스터 스폰 타이밍 체크 (3분, 7분, 13분)
        foreach (float timing in eliteSpawnTimes)
        {
            if (playTime >= timing && !triggeredTimes.Contains(timing))
            {
                SpawnFromPool(eliteMonsterPrefabs);
                triggeredTimes.Add(timing);
            }
        }

        // 중간보스 스폰 타이밍 체크 (12분)
        foreach (float timing in midBossTimes)  
        {
            if (playTime >= timing && !triggeredTimes.Contains(timing))
            {
                SpawnFromPool(midBossPrefabs);
                triggeredTimes.Add(timing);
            }
        }

        // 보스 스테이지면 보스 스폰 (15분)
        if (isBossStage && playTime >= 900f && !bossSpawned)    
        {
            SpawnFromPool(bossPrefabs);
            bossSpawned = true;
        }
    }

    // 일반몬스터 스폰 코루틴
    IEnumerator SpawnRegularMonsters()  
    {
        while (isSpawning)
        {
            Vector3 pos = GetValidSpawnPosition();
            GameObject prefab = regularMonsterPrefabs[Random.Range(0, regularMonsterPrefabs.Length)];
            GameObject monster = PoolManager.Instance.Pop(prefab);
            InitPooledMonster(monster, pos);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 원형진 몬스터 스폰 코루틴
    IEnumerator SurroundSpawnRoutine()  
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(surroundInterval);
            if (playTime >= 900f) yield break;  // 15분까지만

            float radius = (minSpawnDistance + maxSpawnDistance) / 2f;
            GameObject prefab = regularMonsterPrefabs[Random.Range(0, 2)]; // 일반형, 탱커형 중 한마리 랜덤 (추후 확장)

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

    void SpawnFromPool(GameObject[] prefabs)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject monster = PoolManager.Instance.Pop(prefab);
        InitPooledMonster(monster, GetValidSpawnPosition());
    }

    // 스폰 몬스터 필드에 셋팅
    void InitPooledMonster(GameObject monster, Vector3 pos) 
    {
        monster.transform.position = pos;
        monster.transform.rotation = Quaternion.identity;   // 반전처리 해야함
        monster.SetActive(true);

        IMonster resettable = monster.GetComponent<IMonster>(); // 스탯 초기화 (최종보스제외)
        if (resettable != null)
            resettable.ResetMonster();
    }

    /// <summary>
    /// 최종 몬스터 스폰은 연출을 더해서 제작해도 좋을 것 같습니다.
    /// (스폰시키지 않고 최종보스 스테이지씬에 플레이어와 배치해두고 시작)
    /// </summary>

    //void SpawnFinalBoss() 
    //{
    //    Vector3 spawnPos = player.position + Vector3.up * 5f;
    //    spawnedFinalBoss = Instantiate(finalBossPrefab, spawnPos, Quaternion.identity);
    //    finalBossScript = spawnedFinalBoss.GetComponent<MonsterFinalBoss>();
    //    StartCoroutine(FinalBossPhaseMonitor());
    //}
    //IEnumerator FinalBossPhaseMonitor()
    //{
    //    while (spawnedFinalBoss != null)
    //    {
    //        float hpPercent = finalBossScript.CurrentHP / finalBossScript.MaxHP;

    //        if (hpPercent <= 0.3f)
    //        {
    //            SpawnFromPool(midBossPrefabs);
    //            SpawnFromPool(eliteMonsterPrefabs);
    //        }
    //        else if (hpPercent <= 0.6f)
    //        {
    //            SpawnFromPool(eliteMonsterPrefabs);
    //            SpawnFromPool(regularMonsterPrefabs);
    //        }

    //        yield return new WaitForSeconds(10f); // 주기적 체크
    //    }
    //}

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

    // StageManager에서 호출 (스폰 중단)
    public void StopSpawn()  
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    // StageManger에서 호출하여 스테이지 설정
    public void SetupNormalStage()
    {
        isSpawning = true;
        isBossStage = false;
        isFinalBossStage = false;
        StartCoroutine(SpawnRegularMonsters());
        StartCoroutine(SurroundSpawnRoutine());
    }
    public void SetupBossStage()
    {
        isSpawning = true;
        isBossStage = true;
        isFinalBossStage = false;
        StartCoroutine(SpawnRegularMonsters());
        StartCoroutine(SurroundSpawnRoutine());
    }

    public void SetupFinalBossStage()
    {
        isSpawning = true;
        isBossStage = false;
        isFinalBossStage = true;
        // SpawnFinalBoss();
    }

    public void SetupEventStage()
    {
        isSpawning = false;
        isBossStage = false;
        isFinalBossStage = false;
        // NPC랑 연출 관련 부분은 따로 처리
    }
}

