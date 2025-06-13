using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGate : MonoBehaviour
{
    public int dungeonLevel; // 0 ~ 4 사이 값
    public string dungeonSceneName;

    private bool isUnlocked = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer가 없습니다: DungeonGate");
        }

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
            if (GameManager.Instance.dungeonCleared[dungeonLevel - 1])
                isUnlocked = true;
        }

        UpdateGateVisual();
    }

    void UpdateGateVisual()
    {
        if (spriteRenderer == null) return;

        if (isUnlocked)
        {
            spriteRenderer.color = Color.white; // 원래 색
        }
        else
        {
            spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f, 1f); // 어두운 회색
        }
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
