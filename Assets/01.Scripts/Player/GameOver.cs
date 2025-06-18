using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public SaveData data;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ClosePanel()
    {
        Debug.Log("ClosePanel() 함수 실행됨!");

        gameObject.SetActive(false);

        // 게임 다시 진행
        Time.timeScale = 1f;

        Debug.Log("판넬 닫힘! 게임 다시 진행");

        //SaveGame(data);

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
}
