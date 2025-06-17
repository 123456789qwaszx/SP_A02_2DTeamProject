using System.Collections;
using UnityEngine;

public interface IMonster
{
    void ResetMonster();
    void TakeDamage(float amount);
}

public class MonsterBase : MonoBehaviour, IMonster, IDamagable
{
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.1f;

    [Header("사운드 연출")]
    [SerializeField] public AudioClip hitSFX;
    [SerializeField] public AudioClip deathSFX;

    [Header("장판")]
    [SerializeField] protected GameObject warningPrefab;
    
    [Header("드랍 관련")]
    [SerializeField] private float baseDropChance = 5f;   // 기본 드랍 확률
    [SerializeField] private int minDropCount = 1;         // 최소 드랍 수
    [SerializeField] private int maxDropCount = 1;         // 최대 드랍 수 (보스는 3~5 등)

    public MonsterStateIdle StateIdle { get; protected set; }
    public MonsterStateMove StateMove { get; protected set; }
    public MonsterStateAttackMelee StateMeleeAttack { get; private set; }
    public MonsterStateAttackRanged StateRangedAttack { get; private set; }
    public MonsterStateDead StateDead { get; protected set; }

    private MonsterStateMachine stateMachine;
    private ItemDropManager itemDropManager;

    protected float currentHP;
    private float lastAttackTime;

    protected Animator animator;
    protected Transform player;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    private Coroutine hitFlashRoutine;
    private Coroutine knockbackRoutine;
    private Color hitColor = Color.red;
    private float flashDuration = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        itemDropManager = FindObjectOfType<ItemDropManager>();

        stateMachine = new MonsterStateMachine();

        InitializeStates();
    }

    // 보스, 최종보스는 오버라이드
    protected virtual void InitializeStates()
    {
        StateIdle = new MonsterStateIdle(this, animator);
        StateMove = new MonsterStateMove(this, animator);
        StateDead = new MonsterStateDead(this, animator);

        if (IsRangedMonster())
            StateRangedAttack = new MonsterStateAttackRanged(this, animator);
        else
            StateMeleeAttack = new MonsterStateAttackMelee(this, animator);
    }

    private void OnEnable()
    {
        ResetMonster();
    }

    protected virtual void Update()
    {
        stateMachine?.Update();
    }

    // 몬스터 초기화
    public virtual void ResetMonster()
    {
        currentHP = monsterData.maxHP;
        /// 추후 초기화 할 거 있으면 추가
        lastAttackTime = Time.time;
        ChangeState(StateIdle);
    }

    // 피격 메서드
    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;
        // 피격 사운드
        if (hitSFX != null)
            SoundManager.Instance.PlaySFX(hitSFX);
        // 맞으면 반짝
        PlayHitFlash();
        // 넉백
        KnockbackFrom(player.position);

        if (currentHP <= 0)
        {
            ChangeState(StateDead);
        }
    }

    // 맞으면 반짝
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

    // 넉백
    public void KnockbackFrom(Vector3 playerPosition)
    {
        if (knockbackRoutine != null)
            StopCoroutine(knockbackRoutine);

        knockbackRoutine = StartCoroutine(KnockbackRoutine(playerPosition));
    }

    private IEnumerator KnockbackRoutine(Vector3 playerPosition)
    {
        Vector3 knockbackDirection = (transform.position - playerPosition).normalized;
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            transform.position += knockbackDirection * knockbackForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        knockbackRoutine = null;
    }

    // 상태 전환
    public void ChangeState(IMonsterState newState)
    {
        stateMachine.ChangeState(newState);
    }

    // 움직이기
    public void MoveToPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * monsterData.moveSpeed;
    }

    // 플레이어방향으로 좌우반전
    public void Flip()
    {
        Vector2 direction = player.position - transform.position;
        spriteRenderer.flipX = direction.x < 0;
    }

    // 그만 움직이기 (Idle로 전환)
    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Move", false);    // Idle
    }

    // 공격범위 안에 있고 쿨 돌았는지
    public bool CanAttack()
    {
        return InAttackRange() && Time.time >= lastAttackTime + monsterData.attackCooldown;
    }

    // [이벤트함수] MeleeAttack 애니메이션 - 피격
    public void OnAttack()  
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, monsterData.attackRange, targetLayer);

        if (hit != null)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(monsterData.attackPower);
            }
        }
    }

    // [이벤트함수] RangedAttack 애니메이션 - 발사
    public void OnFireProjectile()
    {
        if (monsterData.projectilePrefab == null || player == null) return;

        GameObject proj = Instantiate(monsterData.projectilePrefab, transform.position, Quaternion.identity);
        Vector3 dir = (player.position - transform.position).normalized;
        proj.GetComponent<MonsterProjectile>().SetDirection(dir);
        RecordAttackTime();
    }

    // 공격 범위 체크
    public bool InAttackRange()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= monsterData.attackRange;
    }

    // 공격 쿨 기록
    public void RecordAttackTime()
    {
        lastAttackTime = Time.time;
    }

    // [이벤트함수] Attack 애니메이션 마지막 프레임에 추가
    public virtual void OnAttackEnd()   
    {
        if (CanAttack())
        {
            ChangeState(StateMeleeAttack);
        }
        else if (InAttackRange())
        {
            ChangeState(StateIdle);
        }
        else
        {
            ChangeState(StateMove);
        }
    }

    // [이벤트함수] Dead 애니메이션 마지막 프레임에 추가
    public virtual void OnDeadEnd()
    {
        var player = GameManager.Instance.player;
        float itemAP = player != null ? player.ItemAcquisitionProbability : 0f;

        float bonusChance = baseDropChance * itemAP;
        float finalDropChance = baseDropChance + bonusChance;

        int dropCount = Random.Range(minDropCount, maxDropCount + 1);

        for (int i = 0; i < dropCount; i++)
        {
            itemDropManager.TryDropItem(transform.position, finalDropChance);
        }

        PoolManager.Instance.Push(this.gameObject);
    }

    // 원거리공격몬스터인지
    public bool IsRangedMonster()
    {
        return monsterData.type == MonsterData.MonsterType.Ranged_Normal ||
               monsterData.type == MonsterData.MonsterType.Ranged_Elite;
    }
}
