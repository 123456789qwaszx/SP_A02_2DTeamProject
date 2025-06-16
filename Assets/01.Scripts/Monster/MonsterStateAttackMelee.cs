using UnityEngine;

public class MonsterStateAttackMelee : MonsterStateBase
{
    public MonsterStateAttackMelee(MonsterBase monster, Animator animator) : base(monster, animator) { }

    public override void Enter()
    {
        monster.StopMoving();
        monster.RecordAttackTime();
        animator.SetTrigger("Attack");
    }
}
