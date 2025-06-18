using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("옵션 UI 패널")]
    public GameObject option;

    private bool isPaused = false;

    private void Start()
    {
        // 시작 시 자식 오브젝트인 stopPanel을 비활성화
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // 게임 일시정지
        transform.GetChild(0).gameObject.SetActive(true); // 자식 오브젝트 활성화
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 게임 재개
        transform.GetChild(0).gameObject.SetActive(false); // 자식 오브젝트 비활성화
    }

    public void CancleGame()
    {
        gameObject.SetActive(false);

        // 게임 다시 진행
        Time.timeScale = 1f;

        Debug.Log("판넬 닫힘! 게임 다시 진행");


        // DontDestroyOnLoad로 유지된 오브젝트들 정리
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj != null && obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
                Debug.Log($"삭제된 DontDestroy 객체: {obj.name}");
            }
        }

        // 메인 씬으로 이동
        SceneManager.LoadScene("MainScene");
    }

    public void OpenOptionPanel()
    {
        // Stop 패널을 끄기
        transform.GetChild(0).gameObject.SetActive(false);

        // Option 오브젝트의 첫 번째 자식 오브젝트를 활성화
        GameObject optionChild = option.transform.GetChild(0).gameObject;
        if (optionChild != null)
        {
            optionChild.SetActive(true);
        }
        else
        {
            Debug.LogError("Option 오브젝트의 첫 번째 자식 오브젝트를 찾을 수 없습니다!");
        }
    }
}