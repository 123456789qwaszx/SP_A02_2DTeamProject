using UnityEngine;

public class MonsterStateAttackFinalBoss : MonsterStateBase
{
    private FinalBoss finalBoss;

    public MonsterStateAttackFinalBoss(FinalBoss monster, Animator animator) : base(monster, animator)
    {
        this.finalBoss = monster;
    }

    public override void Enter()
    {
        float hpPercent = finalBoss.HPPercent;

        if (hpPercent > 0.6f)
            animator.SetTrigger("Attack_Phase1"); // 단일 투사체
        else if (hpPercent > 0.3f)
            animator.SetTrigger("Attack_Phase2"); // 3갈래 + 장판
        else
            animator.SetTrigger("Attack_Phase3"); // 장판폭발 or 돌진
    }

    public override void Update()
    {
        if (!monster.CanAttack())
        {
            monster.ChangeState(monster.StateMove);
        }
    }
}
