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

        if (Input.GetKeyDown(KeyCode.X))
        {
            stageTimer += 60f;
            Debug.Log("치트 발동: 1분 추가됨! 현재 시간: " + stageTimer);
        }

        UIManager.Instance.UpdatePlayTime(stageTimer);

        if (Input.GetKeyDown(KeyCode.F))
        {
            ForceClear();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Cheat_ClearAllStages();
        }

        if (currentStageType == StageType.Normal && stageTimer >= 900f)
        {
            StageClear();
        }
    }
    private void Cheat_ClearAllStages()
    {
        for (int i = 0; i < GameManager.Instance.stageCleared.Length; i++)
        {
            GameManager.Instance.stageCleared[i] = true;
        }
        Debug.Log("모든 스테이지 클리어 완료 (치트)");
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

    public float GetStageTime()
    {
        return stageTimer;
    }

    public void StageClear()
    {
        isStageCleared = true;
        monsterSpawnManager.StopSpawn();
        PoolManager.Instance.Clear();

        GameManager.Instance.UpdateStageInfo();

        SceneManager.LoadScene("MainScene");
        GameManager.Instance.player.transform.position = Vector3.zero;
    }

    public void ForceClear()
    {
        StageClear();
    }
}
