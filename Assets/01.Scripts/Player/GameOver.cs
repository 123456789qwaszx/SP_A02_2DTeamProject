using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClosePanel()
    {
        Debug.Log("ClosePanel() 함수 실행됨!");

        gameObject.SetActive(false);

        // 게임 다시 진행 (타임스케일 1)
        Time.timeScale = 1f;

        Debug.Log("판넬 닫힘! 게임 다시 진행");
    }
}
