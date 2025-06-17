using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    public enum StageType { None, Normal, Boss, FinalBoss }

    public StageType currentStageType;

    private MonsterSpawnManager monsterSpawnManager;

    private float stageTimer = 0f;
    private bool isStageCleared = false;

    private void Awake()
    {
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
        monsterSpawnManager = MonsterSpawnManager.Instance;
        SetupStage(GameManager.Instance.currentStage);
    }

    private void Update()
    {
        if (currentStageType == StageType.None || isStageCleared)
            return;

        stageTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ForceClear(); // 디버그용 강제 클리어
        }

        if (currentStageType == StageType.Normal && stageTimer >= 900f)
        {
            StageClear();
        }
        // 보스 및 최종보스는 OnDeadEnd()에서 StageClear 호출
    }

    public void SetupStage(int stage)
    {
        isStageCleared = false;
        stageTimer = 0f;

        SetupStageType(stage);
    }

    void SetupStageType(int stage)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainScene" || sceneName == "TitleScene")
        {
            currentStageType = StageType.None;
        }
        else if (stage == 16)
        {
            currentStageType = StageType.FinalBoss;
        }
        else if (stage == 5 || stage == 10 || stage == 15)
        {
            currentStageType = StageType.Boss;
        }
        else
        {
            currentStageType = StageType.Normal;
        }

        monsterSpawnManager.SetupStage(currentStageType);
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
