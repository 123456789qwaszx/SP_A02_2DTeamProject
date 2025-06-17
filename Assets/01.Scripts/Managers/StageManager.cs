using TMPro;
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
        monsterSpawnManager = MonsterSpawnManager.Instance;
        SetupStage(GameManager.Instance.currentStage);

        GameObject playTimeTxt = GameObject.Find("PlayTimeTxt");
    }

    private void Update()
    {
        if (currentStageType == StageType.None || isStageCleared)
            return;

        stageTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            stageTimer += 60f;
        }

        UIManager.Instance.UpdatePlayTime(stageTimer);

        if (Input.GetKeyDown(KeyCode.F))
        {
            ForceClear();
        }

        if (currentStageType == StageType.Normal && stageTimer >= 900f)
        {
            StageClear();
        }
    }


    public void SetupStage(int stage)
    {
        isStageCleared = false;
        stageTimer = 0f;

        SetupStageType(stage);
    }

    private void SetupStageType(int stage)
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
