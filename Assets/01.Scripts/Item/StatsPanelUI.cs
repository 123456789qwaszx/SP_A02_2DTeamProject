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
    // [SerializeField] private TextMeshProUGUI hpText;
    // [SerializeField] private TextMeshProUGUI mpText;
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
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    
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

        attackText.text = $"공격력: {player.Attack:F2}";
        specialAttackText.text = $"특수 공격력: {player.SpecialAttack:F2}";
        skillAttackText.text = $"스킬 공격력: {player.SkillAttack:F2}";
        defenseText.text = $"방어력: {player.Defense:F2}";
        criticalText.text = $"치명타 확률: {player.Critical * 100f:F1}%";
        critMultiplierText.text = $"치명타 피해량: {player.CritMultiplier:F2}배";

        strText.text = $"힘: {player.STR:F2}";
        dexText.text = $"민첩: {player.DEX:F2}";
        intText.text = $"지능: {player.INT:F2}";
        
        maxHpText.text = $"HP: {player.MaxHP:F2}";
        maxMpText.text = $"MP: {player.MaxMP:F2}";

        hpRegenAmountText.text = $"HP 회복량: {player.HPRegenAmount:F2}/초";
        mpRegenAmountText.text = $"MP 회복량: {player.MPRegenAmount:F2}/초";
        hpRegenIntervalText.text = $"HP 회복 주기: {player.HPRegenInterval:F2}초";
        mpRegenIntervalText.text = $"MP 회복 주기: {player.MPRegenInterval:F2}초";

        hpRecoveryText.text = $"HP 자연회복: {player.HpRecovery:F2}/초";
        mpRecoveryText.text = $"MP 자연회복: {player.MpRecovery:F2}/초";

        moveSpeedText.text = $"이동속도: {player.MoveSpeed:F2}";
        attackSpeedText.text = $"공격속도: {player.AttackSpeed:F2}";

        itemAPText.text = $"아이템 획득률: {player.ItemAcquisitionProbability * 100f:F1}%";

        projectileCountText.text = $"투사체 수: {player.ProjectileCount:F2}";
        projectileSpeedText.text = $"투사체 속도: {player.ProjectileSpeed:F2}";
        attackRangeMultiplierText.text = $"공격 범위 배율: {player.AttackRangeMultiplier:F2}";

    }
}
