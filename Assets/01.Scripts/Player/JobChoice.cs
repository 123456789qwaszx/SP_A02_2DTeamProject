using UnityEngine;

public class JobChoice : MonoBehaviour
{
    public JobChoicePanel jobChoicePanel; // UI 판넬을 관리하는 스크립트

    private void Start()
    {
        if (jobChoicePanel == null)
        {
            jobChoicePanel = GameObject.Find("JobChoicePanel")?.GetComponent<JobChoicePanel>();
            Debug.Log("Find()로 jobChoicePanel 재할당됨: " + jobChoicePanel);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null && !jobChoicePanel.gameObject.activeSelf)
        {
            Debug.Log("플레이어와 충돌 감지됨! 판넬 활성화");
            jobChoicePanel.OpenPanel();
        }
    }
}