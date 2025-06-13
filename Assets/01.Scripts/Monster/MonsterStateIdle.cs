using UnityEngine;

public class MonsterStateIdle : MonsterStateBase
{
    public MonsterStateIdle(MonsterBase monster, Animator animator) : base(monster, animator) { }

    public override void Enter()
    {
        animator.SetBool("Move", false);
    }

    public override void Update()
    {
        monster.Flip();
        if (monster.CanAttack())
        {
            if (monster.IsRangedMonster())
                monster.ChangeState(monster.StateRangedAttack);
            else
                monster.ChangeState(monster.StateMeleeAttack);
        }
        else if (!monster.InAttackRange())
        {
            monster.ChangeState(monster.StateMove);
        }
    }
}
