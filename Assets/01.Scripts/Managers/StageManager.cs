using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    public enum StageType { Normal, Boss, FinalBoss, Event }

    public StageType currentStageType;

    private MonsterSpawnManager monsterSpawnManager;

    private float stageTimer = 0f;
    private bool isStageCleared = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        monsterSpawnManager = MonsterSpawnManager.Instance;
        SetupStage(GameManager.Instance.currentStage);
    }

    private void Update()
    {
        if (isStageCleared) return;

        stageTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            // 디버그용 강제 클리어
            ForceClear();
        }

        if (currentStageType == StageType.Normal && stageTimer >= 900f)
        {
            StageClear();
        }
        // 보스와 최종보스는 죽는 순간에 StageClear가 호출됨 (OnDeadEnd()에서)
    }

    public void SetupStage(int stage)
    {
        isStageCleared = false;
        stageTimer = 0f;

        SetupTheme(stage);
        SetupStageType(stage);
    }

    void SetupStageType(int stage)
    {
        if (stage == 16)
        {
            currentStageType = StageType.FinalBoss;
        }
        else if (stage == 5 || stage == 10 || stage == 15)
        {
            currentStageType = StageType.Boss;
        }
        else if (stage == 3 || stage == 8 || stage == 13)
        {
            currentStageType = StageType.Event;
        }
        else
        {
            currentStageType = StageType.Normal;
        }

        monsterSpawnManager.SetupStage(currentStageType);
    }

    void SetupTheme(int stage)
    {
        if (stage >= 1 && stage <= 4)
        {
            ApplyTheme("Lv.1 Castle");
        }
        else if (stage >= 5 && stage <= 8)
        {
            ApplyTheme("Lv.2 Poison");
        }
        else if (stage >= 9 && stage <= 12)
        {
            ApplyTheme("Lv.3 Desert");
        }
        else if (stage >= 13 && stage <= 15)
        {
            ApplyTheme("Lv.4 GoldGarden");
        }
        else if (stage == 16)
        {
            ApplyTheme("FinalBoss");
        }
    }

    void ApplyTheme(string themeName)
    {
        // AudioManager.Instance.PlayBGM(themeName);
    }

    public void StageClear()
    {
        isStageCleared = true;
        monsterSpawnManager.StopSpawn();
        GameManager.Instance.UpdateStageInfo();

        SceneManager.LoadScene("MainScene");
    }

    public void ForceClear()
    {
        StageClear();
    }
}
