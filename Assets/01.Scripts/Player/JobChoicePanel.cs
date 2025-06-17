using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JobChoicePanel : MonoBehaviour
{
    // 플레이어 직업 프리팹 (Warrior, Archer, Wizard)
    public GameObject playerWarrior;
    public GameObject playerArcher;
    public GameObject playerWizard;

    // 직업 선택 버튼들
    public Button warriorButton;
    public Button archerButton;
    public Button wizardButton;

    private PlayerInput playerInput;

    void Start()
    {
        // 시작할 때 JobChoicePanel UI를 비활성화
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

        // 모든 플레이어 입력 비활성화
        if (playerInput != null)
        {
            playerInput.actions.Disable();
            Debug.Log("판넬 열림!");
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);

        if (playerInput != null)
        {
            playerInput.actions.Enable();
            Debug.Log("모든 플레이어 입력 활성화됨!");
        }

        // 모든 버튼을 리셋
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

    /// <summary>
    /// 기존의 모든 직업 오브젝트를 비활성화하고 선택한 프리팹을 인스턴스화하여 플레이어로 설정
    /// 기존에 "Player" 태그를 가진 객체가 있다면 삭제 후 새 오브젝트를 생성
    /// </summary>
    /// <param name="selectedClass">선택한 직업 프리팹</param>
    public void SetPlayerClass(GameObject selectedClass)
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

        // Instantiate()를 통해 씬에 새 인스턴스를 생성
        GameObject newPlayer = Instantiate(selectedClass, Vector3.zero, Quaternion.identity);

        // 새 플레이어의 태그와 레이어를 설정
        newPlayer.tag = "Player";
        newPlayer.layer = LayerMask.NameToLayer("Player");
        
        // === 새 플레이어 등록 및 장비 로드 ===
        GameManager.Instance.player = newPlayer.GetComponent<Player>();
        PlayerEquipmentManager.Instance.LoadEquipmentForClass(GameManager.Instance.player.CharacterClass);
        
        // 장착 UI 자동 갱신
        ItemEquipHandler.Instance.RefreshUI();

        Debug.Log("새 플레이어 생성됨: " + newPlayer.name + ", 위치: " + newPlayer.transform.position);

        // 직업 전환 전에 기존 AttackEffect 오브젝트 제거
        GameObject[] existingEffects = GameObject.FindGameObjectsWithTag("AttackEffect");
        foreach (GameObject effect in existingEffects)
        {
            Destroy(effect);
        }

        ClosePanel();
    }

    private void UpdateButtonStates(Button selectedButton)
    {
        // 모든 버튼을 리셋
        warriorButton.interactable = true;
        archerButton.interactable = true;
        wizardButton.interactable = true;

        warriorButton.image.color = Color.white;
        archerButton.image.color = Color.white;
        wizardButton.image.color = Color.white;

        // 선택된 버튼 비활성화 및 색상 변경
        selectedButton.interactable = false;
        selectedButton.image.color = Color.gray;
    }
}