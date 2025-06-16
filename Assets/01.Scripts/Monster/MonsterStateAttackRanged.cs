using UnityEngine;

public class MonsterStateAttackRanged : MonsterStateBase
{
    public MonsterStateAttackRanged(MonsterBase monster, Animator animator) : base(monster, animator) { }

    private float lastAttackTime;
    
    public override void Enter()
    {
        monster.StopMoving();
        monster.RecordAttackTime();
        animator.SetTrigger("Attack");
    }

    public override void Update()
    {
        if (!monster.InAttackRange())
        {
            monster.ChangeState(monster.StateMove);
        }
    }
}
