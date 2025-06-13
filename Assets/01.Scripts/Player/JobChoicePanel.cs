using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JobChoicePanel : MonoBehaviour
{
    // 플레이어 직업 오브젝트 (Warrior, Archer, Wizard)
    public GameObject playerWarrior;
    public GameObject playerArcher;
    public GameObject playerWizard;
   
    private PlayerInput playerInput;

    // 직업 선택 버튼 (전사, 궁수, 마법사)
    public Button warriorButton;
    public Button archerButton;
    public Button wizardButton;

    void Start()
    {
        // 시작할 때 직업 선택 UI를 비활성화
        gameObject.SetActive(false);

        // PlayerInput 가져오기
        playerInput = FindObjectOfType<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput을 찾을 수 없습니다! 플레이어 오브젝트를 확인하세요.");
        }
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);

        if (playerInput != null)
        {
            playerInput.actions.Disable(); // 모든 인풋 액션 비활성화
            Debug.Log("판넬 열림!");
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);

        if (playerInput != null)
        {
            playerInput.actions.Enable(); // 모든 인풋 액션 다시 활성화
            Debug.Log("모든 플레이어 입력 활성화됨!");
        }

        // 모든 버튼을 다시 활성화하고 색상을 초기화
        warriorButton.interactable = true;
        archerButton.interactable = true;
        wizardButton.interactable = true;

        warriorButton.image.color = Color.white;
        archerButton.image.color = Color.white;
        wizardButton.image.color = Color.white;

        Debug.Log("판넬 닫힘!");
    }

public void ChooseWarrior()
    {
        // 전사(Warrior) 직업 선택
        SetPlayerClass(playerWarrior);
        UpdateButtonStates(warriorButton);
    }

    public void ChooseArcher()
    {
        // 궁수(Archer) 직업 선택
        SetPlayerClass(playerArcher);
        UpdateButtonStates(archerButton);
    }

    public void ChooseWizard()
    {
        // 마법사(Wizard) 직업 선택
        SetPlayerClass(playerWizard);
        UpdateButtonStates(wizardButton);
    }

    public void SetPlayerClass(GameObject selectedClass)
    {
        // 모든 직업을 비활성화하고 선택된 직업만 활성화
        playerWarrior.SetActive(false);
        playerArcher.SetActive(false);
        playerWizard.SetActive(false);

        selectedClass.SetActive(true);

        // 플레이어 위치를 (0,0,0)으로 초기화
        selectedClass.transform.position = Vector3.zero;
        Debug.Log("플레이어 위치 초기화: " + selectedClass.transform.position);

        ClosePanel(); // 직업 선택 후 UI 닫기
    }




    private void UpdateButtonStates(Button selectedButton)
    {
        // 모든 버튼을 다시 활성화하고 색상을 초기화
        warriorButton.interactable = true;
        archerButton.interactable = true;
        wizardButton.interactable = true;

        warriorButton.image.color = Color.white;
        archerButton.image.color = Color.white;
        wizardButton.image.color = Color.white;

        // 선택된 직업 버튼을 비활성화하고 회색으로 변경
        selectedButton.interactable = false;
        selectedButton.image.color = Color.gray;
    }
}