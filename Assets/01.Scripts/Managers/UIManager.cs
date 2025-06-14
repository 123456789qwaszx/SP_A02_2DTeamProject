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

                    // 스테이지에 따라 다른 테마 씬으로 로드
                    if (stageIndex <= 3)
                    {
                        SceneManager.LoadScene("Dungeon_Lv.1_CastleScene");
                    }
                    else if (stageIndex <= 9)
                    {
                        SceneManager.LoadScene("Dungeon_Lv.2_PoisonScene");
                    }
                    else
                    {
                        SceneManager.LoadScene("Dungeon_Lv.3_DesertScene");
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
