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
        if (monster.CanAttack())
            monster.ChangeState(monster.StateMeleeAttack);
        else
            monster.ChangeState(monster.StateMove);
    }
}
