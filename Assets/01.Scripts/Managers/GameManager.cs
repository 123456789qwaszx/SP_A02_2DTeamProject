using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Enums;

public class GameManager : Singleton<GameManager>
{
    [Header("플레이어 정보")]
    public Player player;
    public PlayerController controller;

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private GameObject playerPrefab;

    private bool playerInitialized = false;

    //[Header("골드 및 UI")]
    public int Gold { get; private set; }
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("스테이지 진행 정보")]
    public int currentStage = 1;
    public int maxUnlockedStage = 1;
    public bool[] stageCleared = new bool[16];

    public SaveManager saveManager;
    public SaveData currentData;

    private Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get => _moveDir;
        set => _moveDir = value;
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        saveManager ??= SaveManager.Instance;
        currentData = saveManager.LoadGame() ?? new SaveData();

        SkillManager.Instance.LoadSkill();

        if (player == null)
        {
            player = Instantiate(playerPrefab).GetComponent<Player>();
            DontDestroyOnLoad(player);
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene" && !playerInitialized)
        {
            Debug.Log("메인씬 진입: 기본 Warrior 생성");
            CreatePlayer(CharacterClass.Warrior);
            playerInitialized = true;
            
            StartCoroutine(DelayedInitializeSlots());
        }

        PlayerEquipmentManager.Instance?.ApplyEquippedItemsToCurrentPlayer();
        ItemEquipHandler.Instance?.RefreshUI();
    }
    
    private IEnumerator DelayedInitializeSlots()
    {
        yield return null; // 한 프레임 쉬고

        ItemEquipHandler.Instance.InitializeSlots();
        ItemEquipHandler.Instance.RefreshUI();
    }

    public void CreatePlayer(CharacterClass cls)
    {
        GameObject prefab = cls switch
        {
            CharacterClass.Warrior => warriorPrefab,
            CharacterClass.Archer => archerPrefab,
            CharacterClass.Wizard => wizardPrefab,
            _ => null
        };

        if (prefab == null) return;

        GameObject playerObj = Instantiate(prefab);
        Debug.Log($"플레이어 인스턴스 생성됨: {playerObj.name}");

        player = playerObj.GetComponent<Player>();
        controller = playerObj.GetComponent<PlayerController>();
    }

    public void ChangePlayer(CharacterClass cls)
    {
        if (player != null)
        {
            PlayerEquipmentManager.Instance.SaveCurrentEquipment(player.CharacterClass);
            Destroy(player.gameObject);
        }

        CreatePlayer(cls);
        ItemEquipHandler.Instance.InitializeSlots();

        PlayerEquipmentManager.Instance.LoadEquipmentForClass(player.CharacterClass);
        ItemEquipHandler.Instance.RefreshUI();

        var goldObj = GameObject.Find("GoldText");
        if (goldObj)
        {
            SetGoldText(goldObj.GetComponent<TextMeshProUGUI>());
        }

        RemoveExistingEffects();
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

        if (stage > maxUnlockedStage)
            maxUnlockedStage = stage;

        UIController.Instance.UpdateStageButtons();
    }

    private void RemoveExistingEffects()
    {
        foreach (var effect in GameObject.FindGameObjectsWithTag("AttackEffect"))
            Destroy(effect);
    }

    public void UpdateSaveData()
    {
        currentData.players = new List<PlayerData>();

        foreach (Player p in FindObjectsOfType<Player>())
        {
            PlayerData psd = new PlayerData
            {
                baseData = p.GetPlayerData().baseData
            };

            if (p.TryGetComponent<Archer>(out var archer))
            {
                psd.archerData = archer.GetArcherData();
                psd.jobType = "Archer";
            }
            else if (p.TryGetComponent<Warrior>(out var warrior))
            {
                psd.warriorData = warrior.GetWarriorData();
                psd.jobType = "Warrior";
            }
            else if (p.TryGetComponent<Wizard>(out var wizard))
            {
                psd.wizardData = wizard.GetWizardData();
                psd.jobType = "Wizard";
            }
            else
            {
                psd.jobType = "None";
            }

            currentData.players.Add(psd);
        }

        currentData.gold = Gold;
        currentData.inventoryItems = InventoryManager.Instance.GetSaveItems();
        currentData.equippedItemsPerClass = PlayerEquipmentManager.Instance.GetAllEquippedItems();
    }

    public void SaveGameData()
    {
        UpdateSaveData();
        saveManager.SaveGame(currentData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGameData();
            Debug.Log("게임 저장됨");
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            currentData = saveManager.LoadGame();
            Gold = currentData.gold;
            InventoryManager.Instance.LoadFromSave(currentData.inventoryItems);
            PlayerEquipmentManager.Instance.LoadAllEquippedItems(currentData.equippedItemsPerClass);
            PlayerEquipmentManager.Instance.ApplyEquippedItemsToCurrentPlayer();
            ItemEquipHandler.Instance.RefreshUI();
            Debug.Log("게임 불러옴");
        }
    }
}