using Unity.VisualScripting;
using UnityEngine;

public class MonsterStateDead : MonsterStateBase
{
    public MonsterStateDead(MonsterBase monster, Animator animator) : base(monster, animator) { }

    public override void Enter()
    {
        animator.SetTrigger("Dead");
    }
}
