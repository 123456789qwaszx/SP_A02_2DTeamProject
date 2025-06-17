using UnityEngine;
using UnityEngine.UI;

public class JobChoicePanel : MonoBehaviour
{
    public GameObject playerWarrior;
    public GameObject playerArcher;
    public GameObject playerWizard;

    public Button warriorButton;
    public Button archerButton;
    public Button wizardButton;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);

        // 게임 일시정지 (타임스케일 0)
        Time.timeScale = 0f;

        Debug.Log("판넬 열림! 게임 일시정지");
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);

        // 게임 다시 진행 (타임스케일 1)
        Time.timeScale = 1f;

        Debug.Log("판넬 닫힘! 게임 다시 진행");

        ResetButtonStates();
    }

    public void ChooseWarrior()
    {
        SetPlayerClass(playerWarrior);
        UpdateButtonStates(warriorButton);
    }

    public void ChooseArcher()
    {
        SetPlayerClass(playerArcher);
        UpdateButtonStates(archerButton);
    }

    public void ChooseWizard()
    {
        SetPlayerClass(playerWizard);
        UpdateButtonStates(wizardButton);
    }

    private void SetPlayerClass(GameObject selectedClass)
    {
        GameObject existingPlayer = GameObject.FindWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer);
        }

        GameObject newPlayer = Instantiate(selectedClass, Vector3.zero, Quaternion.identity);
        newPlayer.tag = "Player";
        newPlayer.layer = LayerMask.NameToLayer("Player");

        Debug.Log("새 플레이어 생성됨: " + newPlayer.name);

        RemoveExistingEffects();
        ClosePanel();
    }

    private void RemoveExistingEffects()
    {
        GameObject[] existingEffects = GameObject.FindGameObjectsWithTag("AttackEffect");
        foreach (GameObject effect in existingEffects)
        {
            Destroy(effect);
        }
    }

    private void UpdateButtonStates(Button selectedButton)
    {
        ResetButtonStates();
        selectedButton.interactable = false;
        selectedButton.image.color = Color.gray;
    }

    private void ResetButtonStates()
    {
        warriorButton.interactable = true;
        archerButton.interactable = true;
        wizardButton.interactable = true;

        warriorButton.image.color = Color.white;
        archerButton.image.color = Color.white;
        wizardButton.image.color = Color.white;
    }
}