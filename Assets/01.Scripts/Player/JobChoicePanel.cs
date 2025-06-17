using TMPro;
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
        // === 기존 장착 상태 저장 ===
        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            PlayerEquipmentManager.Instance.SaveCurrentEquipment(GameManager.Instance.player.CharacterClass);
        }
        
        // 기존의 플레이어 오브젝트(태그가 Player인)를 찾고 삭제합니다.
        GameObject existingPlayer = GameObject.FindWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer);
        }

        GameObject newPlayer = Instantiate(selectedClass, Vector3.zero, Quaternion.identity);
        newPlayer.tag = "Player";
        newPlayer.layer = LayerMask.NameToLayer("Player");
        
        // === 새 플레이어 등록 및 장비 로드 ===
        GameManager.Instance.player = newPlayer.GetComponent<Player>();
        PlayerEquipmentManager.Instance.LoadEquipmentForClass(GameManager.Instance.player.CharacterClass);
        
        // 장착 UI 자동 갱신
        ItemEquipHandler.Instance.RefreshUI();

        Debug.Log("새 플레이어 생성됨: " + newPlayer.name);
        
        var goldObj = GameObject.Find("GoldText");
        if (goldObj != null)
        {
            var goldText = goldObj.GetComponent<TextMeshProUGUI>();
            GameManager.Instance.SetGoldText(goldText);
        }

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