using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public enum StageType { Normal, Boss, FinalBoss, Event }

    public StageType currentStageType;

    [SerializeField] private MonsterSpawnManager monsterSpawnManager;
    private float stageTimer = 0f;
    private bool isStageCleared = false;

    private void Start()
    {
        SetupStage(GameManager.Instance.currentStage);
    }

    private void Update()
    {
        if (isStageCleared) return;

        stageTimer += Time.deltaTime;

        if (currentStageType == StageType.Normal && stageTimer >= 900f) // 일반 스테이지는 15분 버티기
        {
            StageClear();
        }
        else if (currentStageType == StageType.Boss)
        {
            // if 보스가 죽으면
            StageClear();
        }
        else if (currentStageType == StageType.FinalBoss)
        {
            // if 최종 보스가 죽으면
            StageClear();
        }
    }

    public void SetupStage(int stage)
    {
        isStageCleared = false;
        stageTimer = 0f;

        // 스테이지 맵테마 설정
        SetupTheme(stage);

        // 스테이지 타입 결정
        if (stage == 16)
        {
            currentStageType = StageType.FinalBoss;
            monsterSpawnManager.SetupFinalBossStage();
        }
        else if (stage == 5 || stage == 10 || stage == 15)
        {
            currentStageType = StageType.Boss;
            monsterSpawnManager.SetupBossStage();
        }
        else if (stage == 3 || stage == 8 || stage == 13)
        {
            currentStageType = StageType.Event;
            monsterSpawnManager.SetupEventStage();
        }
        else
        {
            currentStageType = StageType.Normal;
            monsterSpawnManager.SetupNormalStage();
        }
    }

    void SetupTheme(int stage)
    {
        if (stage >= 1 && stage <= 5)
        {
            ApplyTheme("Lv.1 Castle");
        }
        else if (stage >= 6 && stage <= 10)
        {
            ApplyTheme("Lv.2 Poison");
        }
        else if (stage >= 11 && stage <= 15)
        {
            ApplyTheme("Lv.3 Desert");
        }
        else if (stage == 16)
        {
            ApplyTheme("FinalBoss");
        }
    }

    void ApplyTheme(string themeName)
    {
        // 추후 오디오매니저 셋팅

        //case "Lv.1 Castle":
        //AudioManager.Instance.PlayBGM("CastleTheme");
        //break;

        //case "Lv.2 Poison":
        //    AudioManager.Instance.PlayBGM("PoisonTheme");
        //    break;

        //case "Lv.3 Desert":
        //    AudioManager.Instance.PlayBGM("DesertTheme");
        //    break;

        //case "FinalBoss":
        //    AudioManager.Instance.PlayBGM("FinalBossTheme");
        //    break;
        //}
    }

    public void StageClear()
    {
        isStageCleared = true;
        monsterSpawnManager.StopSpawn();
        GameManager.Instance.UdateStageInfo();
    }

    public void ForceClear() // 디버그용 강제 클리어
    {
        StageClear();
    }
}
