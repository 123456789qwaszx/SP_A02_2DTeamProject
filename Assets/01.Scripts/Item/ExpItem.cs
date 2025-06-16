using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public int expAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            // PlayerLevel 컴포넌트 가져오기
            PlayerLevel levelSystem = other.GetComponent<PlayerLevel>();
            if (levelSystem != null)
            {
                Debug.Log("경험치 아이템 획득! + " + expAmount);
                levelSystem.AddExperience(expAmount); // 경험치 추가
            }

            Destroy(gameObject); // 아이템 삭제
        }
    }
}
