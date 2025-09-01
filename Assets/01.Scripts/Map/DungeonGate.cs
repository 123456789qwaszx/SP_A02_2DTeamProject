using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.Instance.OpenStageSelectUI();
        }
    }
}
