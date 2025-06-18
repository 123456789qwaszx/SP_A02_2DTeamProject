using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Enums;

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
        GameManager.Instance.ChangePlayer(CharacterClass.Warrior);
        UpdateButtonStates(warriorButton);
        ClosePanel();
    }

    public void ChooseArcher()
    {
        GameManager.Instance.ChangePlayer(CharacterClass.Archer);
        UpdateButtonStates(archerButton);
        ClosePanel();
    }

    public void ChooseWizard()
    {
        GameManager.Instance.ChangePlayer(CharacterClass.Wizard);
        UpdateButtonStates(wizardButton);
        ClosePanel();
    }

    // public void SetPlayerClass(GameObject selectedClassPrefab)
    // {
    //     StartCoroutine(ChangeJobAndInitialize(selectedClassPrefab));
    // }
    //
    //
    // private IEnumerator ChangeJobAndInitialize(GameObject newJobPrefab)
    // {
    //     // 기존 장비 저장
    //     if (GameManager.Instance != null && GameManager.Instance.player != null)
    //     {
    //         PlayerEquipmentManager.Instance.SaveCurrentEquipment(GameManager.Instance.player.CharacterClass);
    //     }
    //
    //     // 2. 기존 플레이어 제거
    //     GameObject existingPlayer = GameObject.FindWithTag("Player");
    //     if (existingPlayer != null)
    //     {
    //         Destroy(existingPlayer);
    //         Debug.Log("기존 플레이어 삭제됨: " + existingPlayer.name);
    //     }
    //
    //     yield return null; // 한 프레임 대기
    //
    //     // 새 플레이어 생성 및 초기화
    //     GameObject newPlayer = Instantiate(newJobPrefab);
    //     DontDestroyOnLoad(newPlayer);
    //
    //     Player playerComponent = newPlayer.GetComponent<Player>();
    //     playerComponent.Initialize(); // 중복 제거 체크를 Start 대신 수동으로 호출
    //
    //     GameManager.Instance.player = playerComponent;
    //     Debug.Log("새 플레이어 생성됨: " + newPlayer.name);
    //
    //     // 장비 및 UI 재설정
    //     PlayerEquipmentManager.Instance.LoadEquipmentForClass(GameManager.Instance.player.CharacterClass);
    //     ItemEquipHandler.Instance.RefreshUI();
    //
    //     // 골드 텍스트 다시 지정
    //     var goldObj = GameObject.Find("GoldText");
    //     if (goldObj != null)
    //     {
    //         var goldText = goldObj.GetComponent<TextMeshProUGUI>();
    //         GameManager.Instance.SetGoldText(goldText);
    //     }
    //
    //     // 이펙트 제거 및 UI 닫기
    //     RemoveExistingEffects();
    //     ClosePanel();
    // }


    // private void RemoveExistingEffects()
    // {
    //     GameObject[] existingEffects = GameObject.FindGameObjectsWithTag("AttackEffect");
    //     foreach (GameObject effect in existingEffects)
    //     {
    //         Destroy(effect);
    //     }
    // }

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