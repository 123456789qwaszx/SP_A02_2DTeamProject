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

    private SaveData data;

    private void Start()
    {
        data = SaveManager.Instance.LoadGame();
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
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.1_CastleScene");
                    }
                    else if (stageIndex <= 7)
                    {
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.2_PoisonScene");
                    }
                    else if (stageIndex <= 11)
                    {
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.3_DesertScene");
                    }
                    else if (stageIndex <= 14)
                    {
                        SaveManager.Instance.SaveGame(data);
                        SceneManager.LoadScene("Dun_Lv.4_GoldScene");
                    }
                    else if (stageIndex == 15)
                    {
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
}
