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
        animator.SetTrigger("Attack");
    }

    public override void Update()
    {
        if (!monster.CanAttack())
        {
            monster.ChangeState(monster.StateIdle);
        }
    }
}
