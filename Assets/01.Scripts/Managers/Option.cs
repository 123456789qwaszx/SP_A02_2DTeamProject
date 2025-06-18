using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    [Header("옵션 UI 프리팹")]
    public GameObject Ending;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        // 시작 시 자식 오브젝트인 옵션 판넬을 비활성화
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 게임 재개
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CreditsOpen()
    {
        // Stop 패널을 끄기
        transform.GetChild(0).gameObject.SetActive(false);

        // Option 오브젝트의 첫 번째 자식 오브젝트를 활성화
        GameObject endingChild = Ending.transform.GetChild(0).gameObject;
        if (endingChild != null)
        {
            endingChild.SetActive(true);
        }
        else
        {
            Debug.LogError("Ending 오브젝트의 첫 번째 자식 오브젝트를 찾을 수 없습니다!");
        }
    }
}
