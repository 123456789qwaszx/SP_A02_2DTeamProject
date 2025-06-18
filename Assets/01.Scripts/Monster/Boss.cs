using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Boss : MonsterBase
{
    [Header("페이즈 전환 효과")]
    [SerializeField] private GameObject phaseChangeEffect;
    [SerializeField] private AudioClip phaseChangeSound;

    

    private bool isDashing = false;
    public bool canDashAttack = true;
    private bool isPhase2 = false;
    public bool IsPhase2 => isPhase2;


    public MonsterStateAttackBoss StateAttackBoss { get; private set; }

    protected override void InitializeStates()
    {
        StateIdle = new MonsterStateIdle(this, animator);
        StateMove = new MonsterStateMove(this, animator);
        StateDead = new MonsterStateDead(this, animator);
        StateAttackBoss = new MonsterStateAttackBoss(this, animator);
    }

    protected override void Update()
    {
        base.Update();

        // 체력 40% 이하 2페이즈
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(currentHP);
            Debug.Log("보스 즉사");
        }
        else if (!isPhase2 && currentHP <= monsterData.maxHP * 0.4f)
        {
            EnterPhase2();
        }
    }

    private void EnterPhase2()
    {
        isPhase2 = true;
        monsterData.attackPower += 10;
        monsterData.moveSpeed += 1;
        monsterData.attackCooldown -= 0.5f;

        //빨갛게
        if (spriteRenderer != null)
        {
            Color newColor = new Color(1f, 0.5f, 0.1f); // 원하는 색
            spriteRenderer.color = newColor;

            // 원래 색상도 덮어쓰기
            originalColor = newColor;
        }
    }

    public override void ResetMonster()
    {
        base.ResetMonster();
        isPhase2 = false;
        canDashAttack = true;
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }

    // [이벤트함수] 공격 애니메이션 끝에 호출
    public override void OnAttackEnd()
    {
        ChangeState(StateIdle);
    }

    // [이벤트함수] 죽음 애니메이션 끝에 호출
    public override void OnDeadEnd()
    {
        base.OnDeadEnd();
        StartCoroutine(ClearStageAfterDelay());
    }

    private IEnumerator ClearStageAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        StageManager.Instance.StageClear();

        PoolManager.Instance.Push(this.gameObject);
    }

    // [이벤트함수] 1페이즈 원거리공격 애니메이션
    public void OnRangedAttack()
    {
        StopMoving();
        // 세 갈래로 발사
        var directions = GetMultiShotDirections();

        // 장판 표시
        ShowWarnings(directions);

        // 0.5초 후 발사
        StartCoroutine(DelayedFire(directions));
    }

    private IEnumerator DelayedFire(List<Vector3> directions)
    {
        yield return new WaitForSeconds(1f);
        FireProjectiles(directions);
    }

    private List<Vector3> GetMultiShotDirections()
    {
        Vector3 baseDir = (player.position - transform.position).normalized;

        List<Vector3> directions = new List<Vector3>();
        directions.Add(baseDir);

        float randomAngle = Random.Range(15f, 30f);
        directions.Add(Quaternion.Euler(0, 0, randomAngle) * baseDir);
        directions.Add(Quaternion.Euler(0, 0, -randomAngle) * baseDir);

        return directions;
    }

    private void ShowWarnings(List<Vector3> directions)
    {
        foreach (Vector3 dir in directions)
        {
            GameObject warning = Instantiate(warningPrefab, transform.position, Quaternion.identity);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            warning.transform.rotation = Quaternion.Euler(0, 0, angle);
            warning.transform.position += dir.normalized * 1f; // 살짝 앞쪽으로

            Destroy(warning, 1f); // 0.5초 후 사라지게
        }
    }
    private void FireProjectiles(List<Vector3> directions)
    {
        foreach (Vector3 dir in directions)
        {
            GameObject proj = Instantiate(monsterData.bossProjectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<MonsterProjectile>().SetDirection(dir.normalized);
        }

        RecordAttackTime();
    }

    // [이벤트함수] 2페이즈 돌진공격 애니메이션
    public void OnDashAttack()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.AddForce(direction * 10f, ForceMode2D.Impulse);
        isDashing = true;
        RecordAttackTime();
        Invoke(nameof(StopDashing), 3f); // 돌진 상태 유지 시간
    }

    private void StopDashing()
    {
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDashing) return;

        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(monsterData.attackPower);
                isDashing = false; // 대시공격 아닐때는 데미지 안입게 함
            }
        }
    }
}
