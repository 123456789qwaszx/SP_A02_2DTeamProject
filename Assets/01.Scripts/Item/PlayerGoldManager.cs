using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    public static PlayerGoldManager Instance;
    
    public int currentGold = 0;
    public TextMeshProUGUI goldText;

    void Awake() => Instance = this;

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    public void SpendGold(int amount)
    {
        currentGold -= amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        goldText.text = $"Gold : {currentGold:N0} G";
    }
}
