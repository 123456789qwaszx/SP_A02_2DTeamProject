using UnityEngine;

public class JobChoice : MonoBehaviour
{
    // Inspector에서 할당할 프리팹 에셋
    public GameObject jobChoicePanel;

    // 씬에 생성된 JobChoicePanel 인스턴스
    private JobChoicePanel Instance;

    private void Start()
    {
        // 만약 인스턴스가 없다면 프리팹을 인스턴스화
        if (Instance == null)
        {
            if (jobChoicePanel != null)
            {
                // Instantiate 후 실제 인스턴스를 얻습니다.
                GameObject instance = Instantiate(jobChoicePanel);
                Instance = instance.GetComponent<JobChoicePanel>();

                // 필요하면 UI Canvas의 자식으로 설정합니다.
                // instance.transform.SetParent(YourCanvas.transform, false);

                Debug.Log("JobChoicePanel 프리팹 인스턴스화됨: " + Instance);
            }
            else
            {
                Debug.LogWarning("JobChoicePanel 프리팹 에셋이 Inspector에 할당되어 있지 않습니다!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체에 Rigidbody2D가 있고 패널 인스턴스가 존재하며 비활성 상태일 때
        if (other.GetComponent<Rigidbody2D>() != null &&
            (Instance != null && !Instance.gameObject.activeSelf))
        {
            Debug.Log("플레이어와 충돌 감지됨! 패널 활성화");
            Instance.OpenPanel();
        }
    }
}