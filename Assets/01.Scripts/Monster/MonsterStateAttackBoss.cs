using UnityEngine;

public class MonsterStateAttackBoss : MonsterStateBase
{
    private Boss boss;

    public MonsterStateAttackBoss(Boss monster, Animator animator) : base(monster, animator)
    {
        this.boss = monster;
    }

    public override void Enter()
    {
        if (boss.IsPhase2)
        {
            if (boss.canDashAttack)
            {
                animator.SetTrigger("Attack_Dash");
            }
            else
            {
                animator.SetTrigger("Attack_Ranged");
            }

            boss.canDashAttack = !boss.canDashAttack;
        }
        else
        {
            animator.SetTrigger("Attack_Ranged");
        }
    }

    public override void Update()
    {
        if (!monster.CanAttack())
        {
            monster.ChangeState(monster.StateMove);
        }
    }
}
