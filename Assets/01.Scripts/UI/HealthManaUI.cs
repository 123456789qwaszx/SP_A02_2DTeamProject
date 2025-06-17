using UnityEngine;
using UnityEngine.UI;

public class HealthManaUI : MonoBehaviour
{
    [Header("슬라이더 참조")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;

    private Player player;

    private float currentHpFill = 1f;
    private float currentMpFill = 1f;

    [SerializeField] private float smoothSpeed = 5f; // 보간 속도

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        player = GameManager.Instance.player;

        if (player == null)
        {
            Debug.LogError("GameManager에서 Player를 참조하지 못했습니다.");
            return;
        }

        if (healthSlider != null) healthSlider.value = 1f;
        if (manaSlider != null) manaSlider.value = 1f;
    }

    private void Update()
    {
        player = GameManager.Instance.player;
        if (player == null) return;

        float currentHP = player.GetHP();
        float currentMP = player.GetMP();
        float maxHP = Mathf.Max(player.GetMaxHP(), 1f);
        float maxMP = Mathf.Max(player.GetMaxMP(), 1f);

        float targetHpRatio = Mathf.Clamp01(currentHP / maxHP);
        float targetMpRatio = Mathf.Clamp01(currentMP / maxMP);

        // 💡 감쇠 함수 기반 보간 (변화량 크면 빨라지고, 작으면 천천히)
        float hpLerpSpeed = 1f - Mathf.Pow(1f - 0.8f, Time.deltaTime * smoothSpeed);
        float mpLerpSpeed = 1f - Mathf.Pow(1f - 0.8f, Time.deltaTime * smoothSpeed);

        currentHpFill = Mathf.Lerp(currentHpFill, targetHpRatio, hpLerpSpeed);
        currentMpFill = Mathf.Lerp(currentMpFill, targetMpRatio, mpLerpSpeed);

        if (healthSlider != null) healthSlider.value = currentHpFill;
        if (manaSlider != null) manaSlider.value = currentMpFill;
    }
}
