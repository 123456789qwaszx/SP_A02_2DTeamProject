using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameManager : Singleton<GameManager>
{
    /* PoolManager 사용 예시
    public GameObject bear_Prefab;
    public GameObject SpawnBear() { return PoolManager.Instance.Pop(bear_Prefab); }
    public void DespawnBear(GameObject bear) { PoolManager.Instance.Push(bear); }
    */

    public Player player;
    public PlayerController controller;

    // 16개의 던전 클리어 여부 저장
    // 예: 저장/로드 시스템과 연동하여 stageCleared를 유지
    public int currentStage = 1;
    public int maxUnlockedStage = 1;
    public bool[] stageCleared = new bool[16];

    public SaveManager saveManager;
    public SaveData currentData;
    
    // 골드 관련
    public int Gold { get; private set; }
    [SerializeField] private TextMeshProUGUI goldText;

    private void Awake()
    {
        // SaveManager가 할당되어 있지 않으면 찾아서 할당
        if (saveManager == null)
        {
            saveManager = FindObjectOfType<SaveManager>();
        }

        // 저장된 데이터 로드
        currentData = saveManager.LoadGame();
        if (currentData == null)
        {
            currentData = new SaveData(); // 데이터가 없으면 새로 생성
        }
        else
        {
            Gold = currentData.gold;
            InventoryManager.Instance.LoadFromSave(currentData.inventoryItems);

            if (currentData.equippedItemsPerClass != null)
            {
                PlayerEquipmentManager.Instance.LoadAllEquippedItems(currentData.equippedItemsPerClass);
            }
        }

        // 씬 전환 시 삭제되지 않도록 함
        DontDestroyOnLoad(gameObject);

        SkillManager.Instance.LoadSkill();
    }
    
    public void AddGold(int amount)
    {
        Gold += amount;
        UpdateGoldUI();
    }

    public bool SpendGold(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        UpdateGoldUI();
        return true;
    }

    public void SetGoldText(TextMeshProUGUI text)
    {
        goldText = text;
        UpdateGoldUI();
    }

    public void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"{Gold:N0} G";
    }

    public void UpdateStageInfo()
    {
        int stage = currentStage;
        stageCleared[stage - 1] = true;

        // 다음 스테이지 언락
        if (stage > maxUnlockedStage)
            maxUnlockedStage = stage;

        UIManager.Instance.UpdateStageButtons();
    }

    Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
        }
    }
    public void UpdateSaveData()
    {
        // SaveData 내의 플레이어 리스트 초기화
        currentData.players = new List<PlayerData>();

        // 모든 플레이어(예: FindObjectsOfType<Player> 사용)
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            PlayerData psd = new PlayerData();
            // 여기서 p.GetPlayerData()는 PlayerData를 반환하므로, 그 안의 baseData를 가져와야 함
            psd.baseData = p.GetPlayerData().baseData;

            // 각 직업 정보 갱신
            if (p.GetComponent<Archer>() != null)
            {
                Archer archer = p.GetComponent<Archer>();
                psd.archerData = archer.GetArcherData();
                psd.jobType = "Archer";
            }
            else if (p.GetComponent<Warrior>() != null)
            {
                Warrior warrior = p.GetComponent<Warrior>();
                psd.warriorData = warrior.GetWarriorData();
                psd.jobType = "Warrior";
            }
            else if (p.GetComponent<Wizard>() != null)
            {
                Wizard wizard = p.GetComponent<Wizard>();
                psd.wizardData = wizard.GetWizardData();
                psd.jobType = "Wizard";
            }
            else
            {
                psd.jobType = "None";
            }

            currentData.players.Add(psd);
        }
        
        currentData.gold = Gold; // 골드 저장
        currentData.inventoryItems = InventoryManager.Instance.GetSaveItems(); // 인벤토리 저장

        // 장착 아이템 저장
        currentData.equippedItemsPerClass = PlayerEquipmentManager.Instance.GetAllEquippedItems();
    }

    // 모든 플레이어 데이터를 갱신한 후 저장하는 함수
    public void SaveGameData()
    {
        // 저장 전에 현재 게임 상태를 최신화
        UpdateSaveData();
        // 현재 최신화된 데이터를 실제 파일에 저장
        saveManager.SaveGame(currentData);
    }

}
