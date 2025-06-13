using System.Collections;
using UnityEngine;
/// <summary>
/// ToDO: 애니메이션 이벤트 함수 등록
/// </summary>
public interface IMonster
{
    void ResetMonster();
    void TakeDamage(int amount);
}

public class MonsterBase : MonoBehaviour, IMonster, IDamagable
{
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.1f;

    private Animator animator;
    private Transform player;

    private MonsterStateMachine stateMachine;
    public MonsterStateIdle StateIdle { get; private set; }
    public MonsterStateMove StateMove { get; private set; }
    public MonsterStateMeleeAttack StateMeleeAttack { get; private set; }
    public MonsterStateDead StateDead { get; private set; }

    private int currentHP;
    private float lastAttackTime;

    private SpriteRenderer spriteRenderer;
    private Coroutine hitFlashRoutine;
    private Coroutine knockbackRoutine;
    private Color hitColor = Color.red;
    private float flashDuration = 0.2f;
    private Color originalColor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        StateIdle = new MonsterStateIdle(this, animator);
        StateMove = new MonsterStateMove(this, animator);
        StateMeleeAttack = new MonsterStateMeleeAttack(this, animator);
        StateDead = new MonsterStateDead(this, animator);

        stateMachine = new MonsterStateMachine();
    }

    private void OnEnable()
    {
        ResetMonster();
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public void ResetMonster()
    {
        currentHP = monsterData.maxHP;
        /// 추후 초기화 할 거 있으면 추가
        lastAttackTime = Time.time;
        ChangeState(StateIdle);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        PlayHitFlash(); // 맞으면 반짝
        KnockbackFrom(player.position); // 넉백

        if (currentHP <= 0)
        {
            ChangeState(StateDead);
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

    public void ChangeState(IMonsterState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void MoveToTarget()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * monsterData.moveSpeed * Time.deltaTime;
    }

    public void Flip()
    {
        Vector2 direction = player.position - transform.position;
        spriteRenderer.flipX = direction.x < 0;
    }

    public void StopMoving()
    {
        animator.SetBool("Move", false);    // Idle
    }

    public bool CanAttack()
    {
        return InAttackRange() && Time.time >= lastAttackTime + monsterData.attackCooldown;
    }
    public void OnAttack()  // Attack 애니메이션 이벤트
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, monsterData.attackRange, playerLayer);

        if (hit != null)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(monsterData.attackPower);
            }
        }
    }

    public bool InAttackRange()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= monsterData.attackRange;
    }

    public void RecordAttackTime()
    {
        lastAttackTime = Time.time;
    }

    public void OnAttackEnd()   // Attack 애니메이션 이벤트
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

    public void OnDeadEnd() // Dead 애니메이션 이벤트
    {
        PoolManager.Instance.Push(this.gameObject);
    }
}
