using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("스테이지 선택 UI 판넬")]
    public GameObject stageSelectPanel;

    [Header("스테이지 버튼들")]
    public GameObject[] stageButtons;

    [Header("닫기 버튼")]
    public Button closeBtn;

    [Header("플레이타임 표시")]
    private TextMeshProUGUI playTimeText;

    [SerializeField] private TextMeshProUGUI warningTxt;

    private Coroutine warningRoutine;
    private SaveData data;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        data = SaveManager.Instance.LoadGame();
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
        playTimeText = GameObject.Find("PlayTimeTxt")?.GetComponent<TextMeshProUGUI>();
    }

    public void UpdatePlayTime(float time)
    {
        if (playTimeText == null) return;

        // 15분에서 멈추기
        if (time >= 900f)
        {
            playTimeText.text = "15:00";
            playTimeText.color = Color.red;
            return;
        }

        int minute = Mathf.FloorToInt(time / 60f);
        int second = Mathf.FloorToInt(time % 60f);
        playTimeText.text = $"{minute:00}:{second:00}";

        // 14분 이상부터 점점 빨개짐
        if (time >= 840f)
        {
            float t = Mathf.InverseLerp(840f, 900f, time);
            playTimeText.color = Color.Lerp(Color.white, Color.red, t);
        }
        else
        {
            playTimeText.color = Color.white;
        }
    }

    public void OpenStageSelectUI()
    {
        stageSelectPanel.SetActive(true);
        UpdateStageButtons();
    }

    public void OnCloseButtonClicked()
    {
        closeBtn.onClick.AddListener(CloseStageSelectUI);
    }

    public void CloseStageSelectUI()
    {
        stageSelectPanel.SetActive(false);
    }

    public void UpdateStageButtons()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int stageIndex = i;

            // 잠금 해제 여부 판단: 첫 번째 스테이지는 항상 열려 있고, 나머지는 이전 스테이지가 클리어되었는지 체크
            bool unlocked = (i == 0) || GameManager.Instance.stageCleared[i - 1];
            stageButtons[i].SetActive(true);

            Button btn = stageButtons[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = unlocked;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    int selectedStage = stageIndex + 1;
                    GameManager.Instance.currentStage = selectedStage;

                    UIManager.Instance.CloseStageSelectUI();

                    // ~4 Stage: Castle
                    if (stageIndex <= 3)
                    {
                        GameManager.Instance.player.transform.position = new Vector3(0, 0, 0);
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.1_CastleScene");
                    }
                    else if (stageIndex <= 7)
                    {
                        GameManager.Instance.player.transform.position = new Vector3(0, 0, 0);
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.2_PoisonScene");
                    }
                    else if (stageIndex <= 11)
                    {
                        GameManager.Instance.player.transform.position = new Vector3(0, 0, 0);
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.3_DesertScene");
                    }
                    else if (stageIndex <= 14)
                    {
                        GameManager.Instance.player.transform.position = new Vector3(0, 0, 0);
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.4_GoldScene");
                    }
                    else if (stageIndex == 15)
                    {
                        GameManager.Instance.player.transform.position = new Vector3(0, 0, 0);
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_FinalScene");
                    }
                });
            }

            Image img = stageButtons[i].GetComponent<Image>();
            if (img != null)
            {
                img.color = unlocked ? Color.white : new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }
    }
    public void ShowWarning(string message, float duration = 2f)
    {
        if (warningRoutine != null)
            StopCoroutine(warningRoutine);

//        warningRoutine = StartCoroutine(WarningRoutine(message, duration));
    }

    private IEnumerator WarningRoutine(string message, float duration)
    {
        warningTxt.text = message;
        warningTxt.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        warningTxt.gameObject.SetActive(false);
    }
}
