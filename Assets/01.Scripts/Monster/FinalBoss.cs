using UnityEngine;

public class FinalBoss : MonsterBase
{
    [Header("최종보스 페이즈 연출")]
    [SerializeField] private GameObject phase2Effect;
    [SerializeField] private GameObject phase3Effect;
    [SerializeField] private AudioClip phaseChangeSound;

    public MonsterStateAttackFinalBoss StateAttackFinalBoss { get; private set; }
    public float HPPercent => (float)currentHP / monsterData.maxHP;

    protected override void Update()
    {
        base.Update();
    }
    protected override void InitializeStates()
    {
        StateIdle = new MonsterStateIdle(this, animator);
        StateMove = new MonsterStateMove(this, animator);
        StateDead = new MonsterStateDead(this, animator);
        StateAttackFinalBoss = new MonsterStateAttackFinalBoss(this, animator);
    }

    public void EnterPhase(int phase)
    {
        switch (phase)
        {
            case 2:
                monsterData.attackPower += 5;
                monsterData.moveSpeed += 1;
                if (phase2Effect != null)
                    Instantiate(phase2Effect, transform.position, Quaternion.identity);
                break;

            case 3:
                monsterData.attackPower += 10;
                monsterData.moveSpeed += 1;
                if (phase3Effect != null)
                    Instantiate(phase3Effect, transform.position, Quaternion.identity);
                break;
        }
    }

    public override void ResetMonster()
    {
        base.ResetMonster();    // HP, 애니메이션, 상태머신 등 공통 리셋
    }

    //public void OnFinalBossAttack()
    //{
    //    float hpPercent = HPPercent;

    //    if (hpPercent > 0.6f)
    //    {
    //        FireSingleProjectile(); // 1페이즈
    //    }
    //    else if (hpPercent > 0.3f)
    //    {
    //        ShowWarningAndFireMulti(); // 2페이즈
    //    }
    //    else
    //    {
    //        StartCoroutine(ExplodingAoE()); // 3페이즈
    //    }

    //    RecordAttackTime();
    //}

    public override void OnDeadEnd()
    {
        base.OnDeadEnd();
        StageManager.Instance.StageClear();
    }
}
