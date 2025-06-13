using UnityEngine;

public class MonsterStateMeleeAttack : MonsterStateBase
{
    public MonsterStateMeleeAttack(MonsterBase monster, Animator animator) : base(monster, animator) { }

    public override void Enter()
    {
        monster.StopMoving();
        monster.RecordAttackTime();
        animator.SetTrigger("Attack");
    }
}
