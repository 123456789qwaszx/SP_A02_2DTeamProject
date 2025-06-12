using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [Header("일반 몬스터")]
    public GameObject[] regularMonsters;
    public GameObject[] eliteMonsters;
    public GameObject midBossMonster;

    [Header("보스")]
    public GameObject[] bosses;
    public GameObject finalBoss;

    [Header("스폰 설정")]
    public float spawnInterval = 2f;
    public float surroundInterval = 120f;
    public float minSpawnDistance = 7f;
    public float maxSpawnDistance = 12f;
    public int surroundCount = 8;

    [Header("스폰 타이밍 트리거")]
    public float[] eliteSpawnTimes = { 180f, 360f, 780f }; // 3분, 6분, 13분
    public float[] midBossTimes = { 600f, 900f }; // 10분, 15분
    public bool isBossStage = false;
    public bool isFinalBossStage = false;

    [Header("최종보스 페이즈 체력 비율")]
    public float phase2Threshold = 0.6f;
    public float phase3Threshold = 0.3f;

    private Transform player;
    private float playTime;
    private HashSet<float> triggeredTimes = new HashSet<float>();
    private GameObject spawnedFinalBoss;
    private bool isSpawning;
    private bool bossSpawned = false;
    private MonsterFinalBoss finalBossScript;


    void Start()
    {
        player = GameManager.Instance.player.transform;

        if (isFinalBossStage)   // 최종보스스테이지냐
        {
            //SpawnFinalBoss();
        }
        else
        {
            StartCoroutine(SpawnRegularMonsters()); // 일반몬스터들 스폰
            StartCoroutine(SurroundSpawnRoutine()); // 원형진 스폰
        }
    }

    void Update()
    {
        if (isFinalBossStage) return;   // 최종보스 스테이지는 별도

        playTime += Time.deltaTime;

        if (!isSpawning) return;    // 스테이지 클리어하면 멈춤

        foreach (float timing in eliteSpawnTimes)   // 엘리트몬스터 스폰 타이밍 체크 (3분, 6분, 13분)
        {
            if (playTime >= timing && !triggeredTimes.Contains(timing))
            {
                SpawnEliteMonster();
                triggeredTimes.Add(timing);
            }
        }

        foreach (float timing in midBossTimes)  // 중간보스 스폰 타이밍 체크 (10분, 15분)
        {
            if (playTime >= timing && !triggeredTimes.Contains(timing))
            {
                SpawnMidBoss();
                triggeredTimes.Add(timing);
            }
        }

        if (isBossStage && playTime >= 900f && !bossSpawned) // 보스스테이지에서만 보스 스폰 (15분)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    IEnumerator SpawnRegularMonsters()  // 일반몬스터 스폰
    {
        while (isSpawning)
        {
            Vector3 pos = GetValidSpawnPosition();
            Instantiate(regularMonsters[Random.Range(0, regularMonsters.Length)], pos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SurroundSpawnRoutine()  // 원형진 몬스터 스폰
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(surroundInterval);

            if (playTime >= 900f) yield break;

            float radius = (minSpawnDistance + maxSpawnDistance) / 2f;

            GameObject chosenMonster = regularMonsters[Random.Range(0, 2)]; // 0,1는 일반몬스터

            for (int i = 0; i < surroundCount; i++)
            {
                float angle = 360f * i / surroundCount;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
                Vector3 pos = player.position + dir * radius;

                if (IsOutsideCameraView(pos))
                    Instantiate(chosenMonster, pos, Quaternion.identity);
            }

        }
    }

    void SpawnEliteMonster()    // 엘리트몬스터 스폰
    {
        Vector3 pos = GetValidSpawnPosition();
        Instantiate(eliteMonsters[Random.Range(0, eliteMonsters.Length)], pos, Quaternion.identity);
    }

    void SpawnMidBoss() // 중간보스 스폰
    {
        Vector3 pos = GetValidSpawnPosition();
        Instantiate(midBossMonster, pos, Quaternion.identity);
    }

    void SpawnBoss()    // 보스 스폰
    {
        Vector3 pos = GetValidSpawnPosition();
        Instantiate(bosses[Random.Range(0, bosses.Length)], pos, Quaternion.identity);
    }

    //void SpawnFinalBoss()   // 최종 보스 스폰
    //{
    //    spawnedFinalBoss = Instantiate(finalBoss, player.position + Vector3.up * 5f, Quaternion.identity);
    //    finalBossScript = spawnedFinalBoss.GetComponent<MonsterFinalBoss>();
    //    StartCoroutine(FinalBossPhaseMonitor());
    //}

    //IEnumerator FinalBossPhaseMonitor() // 최종보스 체력 체크
    //{
    //    while (spawnedFinalBoss != null)
    //    {
    //        float hpPercent = finalBossScript.CurrentHP / finalBossScript.MaxHP;

    //        if (hpPercent <= phase3Threshold)
    //        {
    //            SpawnMidBoss();
    //            SpawnEliteMonster();
    //        }
    //        else if (hpPercent <= phase2Threshold)
    //        {
    //            SpawnEliteMonster();
    //            SpawnRegularMonsters();
    //        }

    //        yield return new WaitForSeconds(10f);
    //    }
    //}

    void OnStageClear() // 스테이지 클리어하면 스폰 중지
    {
        isSpawning = false;
    }

    Vector3 GetValidSpawnPosition() // 스폰 위치 선정
    {
        for (int i = 0; i < 30; i++)
        {
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Vector3 pos = player.position + dir * distance;

            if (IsOutsideCameraView(pos))
                return pos;
        }
        return player.position + Vector3.up * minSpawnDistance;
    }

    bool IsOutsideCameraView(Vector3 pos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(pos);
        return viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1;
    }
}

