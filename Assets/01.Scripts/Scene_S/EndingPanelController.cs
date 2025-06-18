using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingPanelController : MonoBehaviour
{
    [Header("타이틀 버튼")]
    public Button returnToTitleButton;

    private void Start()
    {
        // 버튼에 클릭 이벤트 연결
        if (returnToTitleButton != null)
        {
            returnToTitleButton.onClick.AddListener(OnReturnToTitleClicked);
        }
        else
        {
            Debug.LogError("returnToTitleButton이 할당되지 않았습니다.");
        }
    }

    private void OnReturnToTitleClicked()
    {
        Debug.Log("타이틀 씬으로 이동");

        // 페이드 매니저가 있으면 페이드 아웃하면서 씬 전환
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeOutAndLoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
