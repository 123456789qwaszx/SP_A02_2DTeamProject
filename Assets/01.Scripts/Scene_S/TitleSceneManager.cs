using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleSceneManager : MonoBehaviour
{
    // 시작할 메인 씬 이름
    [SerializeField] private string mainSceneName = "MainScene"; // 실제 메인 씬 이름으로 변경

    // 게임 시작 버튼에 연결
    public void OnClickStartGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    // 게임 종료 버튼에 연결
    public void OnClickQuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("에디터에서 게임 종료");
        EditorApplication.isPlaying = false;
#else
        Debug.Log(" 실제 빌드에서 게임 종료");
        Application.Quit();
#endif
    }

    // 향후 구현: 설정 버튼
    public void OnClickSettings()
    {
        Debug.Log("⚙️ 설정 UI 열기 (추후 구현 예정)");
    }
}
