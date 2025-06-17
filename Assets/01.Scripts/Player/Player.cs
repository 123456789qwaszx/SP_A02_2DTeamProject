using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;

public interface IEquipable
{
    void ApplyOption(ItemOptionType type, float value);
    void RemoveOption(ItemOptionType type, float value);
}
public interface IDamagable
{
    void TakeDamage(float damage);
}

public class Player : MonoBehaviour, IEquipable, IDamagable
{
    public GameObject gameOverPanel;

    protected Rigidbody2D rb;
    protected Transform tf;

    public Rigidbody2D Rb => rb;
    public Transform Tf => tf;
    private CapsuleCollider col;



    public float GetHP() => hp;
    public float GetMP() => mp;
    public float GetMaxHP() => maxHP; // 임시값, 나중에 성장 시스템 연동 가능
    public float GetMaxMP() => maxMP;

    [Header("플레이어 스탯")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float str;               // 힘
    [SerializeField] protected float dex;               // 민첩
    [SerializeField] protected float ints;               // 지능
    [SerializeField] protected float hp;
    [SerializeField] protected float mp;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float maxMP;
    [SerializeField] protected float hpRegenAmount;  // HP회복량 증가
    [SerializeField] protected float mpRegenAmount;  // MP회복량 증가
    [SerializeField] protected float hpRegenInterval; // HP회복속도 증가
    [SerializeField] protected float mpRegenInterval; // MP회복속도 증가
    [SerializeField] protected float itemAP;            // Item Acquisition Probability 아이템 획득 확률

    [SerializeField] protected float projectileSpeed = 1.0f;  // 투사체 속도 배율
    [SerializeField] protected int projectileCount = 1;       // 투사체 수
    [SerializeField] protected float attackRangeMultiplier = 1.0f; // 공격 범위 배율

    [Header("전투 스탯")]
    [SerializeField] protected float attack;           // 공격력
    [SerializeField] protected float specialAttack;    // 특수 공격력
    [SerializeField] protected float skillAttack;      // 특수 공격력
    [SerializeField] protected float defense;          // 방어력
    [SerializeField] protected float critical;         // 크리티컬 확률 (%)
    [SerializeField] protected float critMultiplier;   // 치명타 피해량 증가

    [Header("회복 스탯")]
    [SerializeField] public float hpRecovery;
    [SerializeField] public float mpRecovery;

    public float MoveSpeed => moveSpeed;
    public float AttackSpeed => attackSpeed;
    public float STR => str;
    public float DEX => dex;
    public float INT => ints;
    public float HP => hp;
    public float MP => mp;
    public float MaxHP => maxHP;
    public float MaxMP => maxMP;
    public float HPRegenAmount => hpRegenAmount;
    public float MPRegenAmount => mpRegenAmount;
    public float HPRegenInterval => hpRegenInterval;
    public float MPRegenInterval => mpRegenInterval;
    public float ItemAcquisitionProbability => itemAP;

    public float ProjectileSpeed => projectileSpeed;
    public int ProjectileCount => projectileCount;
    public float AttackRangeMultiplier => attackRangeMultiplier;
    // 전투 스탯
    public float Attack => attack;
    public float SpecialAttack => specialAttack;
    public float SkillAttack => skillAttack;
    public float Defense => defense;
    public float Critical => critical;
    public float CritMultiplier => critMultiplier;

    // 회복 스탯
    public float HpRecovery => hpRecovery;
    public float MpRecovery => mpRecovery;

    public CharacterClass CharacterClass { get; protected set; } = CharacterClass.Warrior;

    SpriteRenderer spriteRenderer;
    private Coroutine hitFlashRoutine;
    private Color originalColor;
    private Color hitColor = Color.red;
    private float flashDuration = 0.2f;




    void Awake()
    {
        GameManager.Instance.player = this;
    }

    void Start()
    {
        // DontDestroyOnLoad 적용으로 씬 전환 시에도 삭제되지 않음
        Player[] uis = FindObjectsOfType<Player>();

        if (uis.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // 씬 전환 시 자동으로 (0,0,0) 위치로 재배치
        transform.position = Vector3.zero;

        GameManager.Instance.player = this;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        if (Rb == null)
            Debug.LogError("Rigidbody not found on Player!");

        col = GetComponent<CapsuleCollider>();

        if (gameOverPanel == null)
        {
            GameObject prefab = Resources.Load<GameObject>("GameOverPanel");
            if (prefab != null)
            {
                gameOverPanel = Instantiate(prefab);
                Debug.Log("GameOverPanel 프리팹 인스턴스 생성됨.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 디버그용 속도업
            moveSpeed *= 2f;
        }
    }

    public void ApplyOption(ItemOptionType type, float value)
    {
        switch (type)
        {
            case ItemOptionType.공격력: attack += value; break;
            case ItemOptionType.방어력: defense += value; break;
            case ItemOptionType.치명타확률: critical += value; break;
            case ItemOptionType.치명타피해량: critMultiplier += value; break;
            case ItemOptionType.힘: str += value; break;
            case ItemOptionType.민첩: dex += value; break;
            case ItemOptionType.지능: ints += value; break;
            case ItemOptionType.공격속도: attackSpeed += value; break;
            case ItemOptionType.이동속도: moveSpeed += value; break;
            case ItemOptionType.행운: itemAP += value; break;
            case ItemOptionType.특수공격력증가: specialAttack += value; break;
            case ItemOptionType.최대HP량증가: maxHP += value; break;
            case ItemOptionType.최대MP량증가: maxMP += value; break;
            case ItemOptionType.초당HP회복량증가: hpRegenAmount += value; break;
            case ItemOptionType.초당MP회복량증가: mpRegenAmount += value; break;
            case ItemOptionType.초당HP회복속도증가: hpRegenInterval *= (1f - value); break;
            case ItemOptionType.초당MP회복속도증가: mpRegenInterval *= (1f - value); break;
            case ItemOptionType.투사체수증가: projectileCount += (int)value; break;
            case ItemOptionType.투사체속도증가: projectileSpeed += value; break; // 배율식 증가
            case ItemOptionType.공격범위증가: attackRangeMultiplier += value; break; // 예: 1.2배
                                                                               // 필요한 항목만 점차 추가해도 됨
        }
    }

    public void RemoveOption(ItemOptionType type, float value)
    {
        ApplyOption(type, -value);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        hp = Mathf.Max(hp, 0);

        PlayHitFlash();

        if (hp <= 0)
        {
            Die();
        }
    }
    private void PlayHitFlash()
    {
        if (hitFlashRoutine != null)
            StopCoroutine(hitFlashRoutine);

        hitFlashRoutine = StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = hitColor;

        float timer = 0f;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float t = timer / flashDuration;
            spriteRenderer.color = Color.Lerp(hitColor, originalColor, t);
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        Debug.Log("Player has died.");

        // 게임 멈추기
        Time.timeScale = 0f;

        // GameOverPanel 프리팹 인스턴스 생성 및 활성화
        if (gameOverPanel != null)
        {
            GameObject instance = Instantiate(gameOverPanel);
            instance.SetActive(true);
            Debug.Log("GameOverPanel 인스턴스 생성됨.");
        }
        else
        {
            Debug.LogError("GameOverPanel 프리팹이 할당되지 않았습니다! Inspector를 확인하세요.");
        }
    }

    // 기본 스탯만 수집하는 헬퍼 메서드
    public BasePlayerData GetBasePlayerData()
    {
        BasePlayerData baseData = new BasePlayerData();
        baseData.moveSpeed = moveSpeed;
        baseData.attackSpeed = attackSpeed;
        baseData.str = str;
        baseData.dex = dex;
        baseData.ints = ints;
        baseData.hp = hp;
        baseData.mp = mp;
        baseData.maxHP = maxHP;
        baseData.maxMP = maxMP;
        baseData.hpRegenAmount = hpRegenAmount;
        baseData.mpRegenAmount = mpRegenAmount;
        baseData.hpRegenInterval = hpRegenInterval;
        baseData.mpRegenInterval = mpRegenInterval;
        baseData.itemAP = itemAP;
        baseData.projectileSpeed = projectileSpeed;
        baseData.projectileCount = projectileCount;
        baseData.attackRangeMultiplier = attackRangeMultiplier;
        baseData.attack = attack;
        baseData.specialAttack = specialAttack;
        baseData.skillAttack = skillAttack;
        baseData.defense = defense;
        baseData.critical = critical;
        baseData.critMultiplier = critMultiplier;
        baseData.hpRecovery = hpRecovery;
        baseData.mpRecovery = mpRecovery;

        return baseData;
    }

    public virtual PlayerData GetPlayerData()
    {
        PlayerData data = new PlayerData();
        data.baseData = GetBasePlayerData(); // GetBasePlayerData()는 BasePlayerData를 반환
        return data;
    }

}