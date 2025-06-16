using UnityEngine;
using UnityEngine.UI;

public class HealthManaUI : MonoBehaviour
{
    public Image healthFillImage;
    public Image manaFillImage;

    private Player player;

    private float maxHP = 100f;
    private float maxMP = 100f;

    private float currentHpFill;
    private float currentMpFill;

    [SerializeField] private float smoothSpeed = 5f; // 부드럽게 전환되는 속도

    private void Awake()
{
    DontDestroyOnLoad(gameObject);
}


    private void Start()
    {
        player = FindObjectOfType<Player>();

        if (player != null)
        {
            maxHP = player.GetMaxHP();
            maxMP = player.GetMaxMP();
        }

        // 초기값 설정
        currentHpFill = 1f;
        currentMpFill = 1f;
    }

    private void Update()
    {
        if (player == null) return;

        float targetHpFill = Mathf.Clamp01(player.GetHP() / maxHP);
        float targetMpFill = Mathf.Clamp01(player.GetMP() / maxMP);

        // 부드럽게 보간
        currentHpFill = Mathf.Lerp(currentHpFill, targetHpFill, Time.deltaTime * smoothSpeed);
        currentMpFill = Mathf.Lerp(currentMpFill, targetMpFill, Time.deltaTime * smoothSpeed);

        healthFillImage.fillAmount = currentHpFill;
        manaFillImage.fillAmount = currentMpFill;
    }
}
