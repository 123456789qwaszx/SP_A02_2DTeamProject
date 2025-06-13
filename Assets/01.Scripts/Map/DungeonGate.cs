using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGate : MonoBehaviour
{
    public int dungeonLevel; // 0 ~ 4 사이 값
    public string dungeonSceneName;
    public GameObject lockOverlay; // 잠금 이미지 또는 이펙트

    private bool isUnlocked = false;

    private void Start()
    {
        CheckUnlockCondition();
    }

    void CheckUnlockCondition()
    {
        // 0레벨 던전은 항상 해제
        if (dungeonLevel == 0)
        {
            isUnlocked = true;
        }
        else
        {
            // 이전 레벨이 클리어 되었는지 확인
            if (GameManager.Instance.dungeonCleared[dungeonLevel - 1])
                isUnlocked = true;
        }

        if (lockOverlay != null)
            lockOverlay.SetActive(!isUnlocked);
    }

   
private void OnTriggerEnter2D(Collider2D other)
{
    if (!isUnlocked) return;

    if (other.CompareTag("Player"))
    {
        GameManager.Instance.currentDungeonLevel = dungeonLevel;
        SceneManager.LoadScene(dungeonSceneName);
    }
}

}
