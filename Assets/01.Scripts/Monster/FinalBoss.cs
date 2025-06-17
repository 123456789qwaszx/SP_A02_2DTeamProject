using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBoss : MonsterBase
{
    [Header("최종보스 페이즈 연출")]
    [SerializeField] private Cinemachine.CinemachineImpulseSource impulseSource;
    [SerializeField] private GameObject phase2Effect;
    [SerializeField] private GameObject phase3Effect;
    [SerializeField] private AudioClip phaseChangeSound;

    [Header("최종보스용 이펙트")]
    [SerializeField] private GameObject largeWarningPrefab; // 3페이즈 경고 장판
    [SerializeField] private GameObject explosionEffectPrefab; // 3페이즈 공격 이펙트
    [SerializeField] private float aoeRadius = 1.5f;

    public MonsterStateAttackFinalBoss StateAttackFinalBoss { get; private set; }
    public float HPPercent => (float)currentHP / monsterData.maxHP;
    private bool isPhase2 = false;
    public bool IsPhase2 => isPhase2;
    private bool isPhase3 = false;
    public bool IsPhase3 => isPhase3;

    private bool tryMultiShot = true;
    private bool tryExplodingAoE = true;
    private bool isInvincible = false;

    protected override void Update()
    {
        base.Update();
        if (!isPhase2 && HPPercent <= 0.6f)
        {
            EnterPhase(2);
        }
        else if (!isPhase3 && HPPercent <= 0.3f)
        {
            EnterPhase(3);
        }   
    }

    protected override void InitializeStates()
    {
        StateIdle = new MonsterStateIdle(this, animator);
        StateMove = new MonsterStateMove(this, animator);
        StateDead = new MonsterStateDead(this, animator);
        StateAttackFinalBoss = new MonsterStateAttackFinalBoss(this, animator);
    }

    public override void ResetMonster()
    {
        base.ResetMonster();    // HP, 애니메이션, 상태머신 등 공통 리셋
        isPhase2 = false;
        isPhase3 = false;
        tryMultiShot = true;
        tryExplodingAoE = true;
    }

    public void EnterPhase(int phase)
    {
        switch (phase)
        {
            case 2:
                isPhase2 = true;
                monsterData.attackPower += 10;
                monsterData.moveSpeed += 1;
                if (phase2Effect != null)
                    Instantiate(phase2Effect, transform.position, Quaternion.identity);
                // 약간 붉은 색상
                if (spriteRenderer != null)
                    spriteRenderer.color = new Color(1f, 0.7f, 0.7f);
                break;

            case 3:
                isPhase3 = true;
                monsterData.attackPower += 20;
                monsterData.moveSpeed += 1;
                if (phase3Effect != null)
                    Instantiate(phase3Effect, transform.position, Quaternion.identity);
                // 더 붉은 색상
                if (spriteRenderer != null)
                    spriteRenderer.color = new Color(1f, 0.4f, 0.4f);
                break;
        }

        SoundManager.Instance.PlaySFX(phaseChangeSound);
        // 일시정지 및 일시 무적 상태
        StartCoroutine(PhaseTransitionRoutine());
    }
    private IEnumerator PhaseTransitionRoutine()
    {
        isInvincible = true;
        StopMoving();

        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        // 4초간 정지
        yield return new WaitForSeconds(4f);

        isInvincible = false;
    }

    // [이벤트함수] 최종보스 공격
    public void OnFinalBossAttack()
    {
        // 1페이즈
        if (HPPercent > 0.6f)
        {
            FireSingleProjectile();
        }
        // 2페이즈
        else if (HPPercent > 0.3f)
        {
            if (tryMultiShot)
            {
                StartCoroutine(FireMulti());
            }
            else
            {
                FireSingleProjectile();
            }
            // 토글
            tryMultiShot = !tryMultiShot;
        }
        // 3페이즈
        else
        {
            if (tryExplodingAoE)
            {
                StartCoroutine(ExplodingAoE());
            }
            else
            {
                StartCoroutine(FireMulti());
            }
            // 토글
            tryExplodingAoE = !tryExplodingAoE;
        }
        RecordAttackTime();
    }

    // 1페이즈: 단일 투사체 발사
    public void FireSingleProjectile()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        GameObject proj = Instantiate(monsterData.bossProjectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<MonsterProjectile>().SetDirection(direction);
    }

    // 2페이즈: 다중 투사체 발사
    public IEnumerator FireMulti()
    {
        var directions = GetMultiShotDirections();
        ShowWarnings(directions);
        yield return new WaitForSeconds(1f); // 경고 후 발사

        foreach (var dir in directions)
        {
            GameObject proj = Instantiate(monsterData.bossProjectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<MonsterProjectile>().SetDirection(dir.normalized);
        }
    }

    private List<Vector3> GetMultiShotDirections()
    {
        Vector3 baseDir = (player.position - transform.position).normalized;
        List<Vector3> directions = new List<Vector3>();

        float angleOffset = 20f; // 양쪽 퍼짐 각도
        directions.Add(baseDir);
        directions.Add(Quaternion.Euler(0, 0, angleOffset) * baseDir);
        directions.Add(Quaternion.Euler(0, 0, -angleOffset) * baseDir);

        return directions;
    }

    private void ShowWarnings(List<Vector3> directions)
    {
        foreach (var dir in directions)
        {
            GameObject warning = Instantiate(warningPrefab, transform.position, Quaternion.identity);
            warning.transform.position += dir.normalized * 1f;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            warning.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(warning, 1f); // 경고 장판 1초 지속
        }
    }

    // 3페이즈: 폭발 AoE 공격
    public IEnumerator ExplodingAoE()
    {
        GameObject warning = Instantiate(largeWarningPrefab, player.position, Quaternion.identity);
        Vector3 explodePos = warning.transform.position;

        yield return new WaitForSeconds(1.2f); // 예고 시간

        GameObject explosion = Instantiate(explosionEffectPrefab, explodePos, Quaternion.identity);

        Collider2D hit = Physics2D.OverlapCircle(explodePos, aoeRadius, targetLayer);
        if (hit != null)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
                damagable.TakeDamage(monsterData.attackPower + 10);
        }

        Destroy(warning);
        Destroy(explosion, 1f);
    }
    public override void TakeDamage(float amount)
    {
        if (isInvincible) return;
        base.TakeDamage(amount);
    }

    public override void OnDeadEnd()
    {
        base.OnDeadEnd();
        StageManager.Instance.StageClear();
        SceneManager.LoadScene("EndingScene");
    }
}
