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
            animator.SetTrigger("Attack_Dash"); // 돌격 애니메이션
        else
            animator.SetTrigger("Attack_Ranged"); // 원거리 애니메이션
    }

    public override void Update()
    {
        if (!monster.CanAttack())
        {
            monster.ChangeState(monster.StateMove);
        }
    }
}
