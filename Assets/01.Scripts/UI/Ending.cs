using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        // 시작 시 자식 오브젝트인 옵션 판넬을 비활성화
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 게임 재개
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
