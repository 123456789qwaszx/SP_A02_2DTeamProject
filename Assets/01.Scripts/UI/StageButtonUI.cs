using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButtonUI : MonoBehaviour
{
    [Tooltip("스테이지 번호")]
    public int stageNumber;

    [Tooltip("스테이지가 속한 씬 이름")]
    public string targetSceneName;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnClickStageButton);
    }

    void OnClickStageButton()
    {
        GameManager.Instance.currentStage = stageNumber;
        SceneManager.LoadScene(targetSceneName);
    }
}
