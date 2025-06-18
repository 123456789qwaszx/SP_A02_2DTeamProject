using UnityEngine;

public class PauseManager : UIManager
{
    [Header("정지 UI 판넬")]
    public GameObject stopPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // 게임 타임스케일 정지
        stopPanel.SetActive(true); // Stop UI 활성화
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // 게임 재시작
        stopPanel.SetActive(false); // Stop UI 비활성화
    }
}