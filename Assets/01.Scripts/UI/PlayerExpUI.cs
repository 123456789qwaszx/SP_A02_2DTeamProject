using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExpUI : MonoBehaviour
{
    public Slider expSlider;
    public TextMeshProUGUI levelText;

    private PlayerLevel playerLevel;

    private float currentExpFill = 0f;
    [SerializeField] private float smoothSpeed = 5f; // 부드럽게 전환되는 속도

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerLevel = FindObjectOfType<PlayerLevel>();

        if (playerLevel == null)
        {
            Debug.LogError("PlayerLevel 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 초기값 설정
        currentExpFill = 0f;
        expSlider.value = 0f;
        
    }

    private void Update()
    {
        if (playerLevel == null) return;

        int currentLevel = playerLevel.CurrentLevel;
        int currentExp = playerLevel.CurrentExp;
        int expToNext = GetExpToNextLevel(currentLevel);

        levelText.text = $"Lv. {currentLevel}";
        expSlider.maxValue = expToNext;

        // 부드러운 보간 처리
        float targetFill = Mathf.Clamp01((float)currentExp / expToNext);
        currentExpFill = Mathf.Lerp(currentExpFill, targetFill, Time.deltaTime * smoothSpeed);
        expSlider.value = currentExpFill * expToNext;
    }

    int GetExpToNextLevel(int level)
    {
        return typeof(PlayerLevel)
            .GetField("expRequirements", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerLevel) is int[] expRequirements && level < expRequirements.Length
            ? expRequirements[level]
            : 1;
    }
}
