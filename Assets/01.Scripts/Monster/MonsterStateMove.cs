using UnityEngine;

public class MonsterStateMove : MonsterStateBase
{
    public MonsterStateMove(MonsterBase monster, Animator animator) : base(monster, animator) { }

    public override void Enter()
    {
        animator.SetBool("Move", true);
    }

    public override void Update()
    {
        monster.MoveToPlayer();
        monster.Flip();
        if (monster.CanAttack())
        {
            if (monster is FinalBoss finalBoss && finalBoss.StateAttackFinalBoss != null)
            {
                monster.ChangeState(finalBoss.StateAttackFinalBoss);
            }
            else if (monster is Boss boss && boss.StateAttackBoss != null)
            {
                monster.ChangeState(boss.StateAttackBoss);
            }
            else
            {
                if (monster.IsRangedMonster())
                    monster.ChangeState(monster.StateRangedAttack);
                else
                    monster.ChangeState(monster.StateMeleeAttack);
            }
        }
    }

    public override void Exit()
    {
        animator.SetBool("Move", false);
    }
}
