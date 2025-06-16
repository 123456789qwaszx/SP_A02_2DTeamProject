using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanelUI : MonoBehaviour
{
    public GameObject statsPanel;
    public Button statsButton;

    [SerializeField] private TextMeshProUGUI strText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    [SerializeField] private TextMeshProUGUI maxHpText;
    [SerializeField] private TextMeshProUGUI maxMpText;
    [SerializeField] private TextMeshProUGUI hpRegenAmountText;
    [SerializeField] private TextMeshProUGUI mpRegenAmountText;
    [SerializeField] private TextMeshProUGUI hpRegenIntervalText;
    [SerializeField] private TextMeshProUGUI mpRegenIntervalText;
    [SerializeField] private TextMeshProUGUI itemAPText;
    [SerializeField] private TextMeshProUGUI hpRecoveryText;
    [SerializeField] private TextMeshProUGUI mpRecoveryText;
    
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI specialAttackText;
    [SerializeField] private TextMeshProUGUI skillAttackText;
    [SerializeField] private TextMeshProUGUI criticalText;
    [SerializeField] private TextMeshProUGUI critMultiplierText;
    
    [SerializeField] private TextMeshProUGUI projectileSpeedText;
    [SerializeField] private TextMeshProUGUI projectileCountText;
    [SerializeField] private TextMeshProUGUI attackRangeMultiplierText;
    
    private Player player;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        statsButton.onClick.AddListener(ToggleStatsPanel);
        statsPanel.SetActive(false);
    }

    private void ToggleStatsPanel()
    {
        bool isActive = statsPanel.activeSelf;
        statsPanel.SetActive(!isActive);

        if (!isActive)
        {
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        if(player == null) return;

        strText.text = $"힘 : {player.STR}";
        dexText.text = $"<UNK> : {player.DEX}";
        intText.text = $"<UNK> : {player.INT}";
        
        attackText.text = $"공격력 : {player.Attack}";
        defenseText.text = $"방어력 : {player.Defense}";

    }
}
