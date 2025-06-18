using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterStateDead : MonsterStateBase
{
    public MonsterStateDead(MonsterBase monster, Animator animator) : base(monster, animator) { }

    public override void Enter()
    {
        if (monster.deathSFX != null)
            SoundManager.Instance.PlaySFX(monster.deathSFX);
        animator.SetTrigger("Dead");

        if (monster is FinalBoss finalBoss || monster is Boss boss)
        {
            Debug.Log("보스 처치! 몬스터 제거");
            ObjectManager.Instance.RemoveMonster();
        }
    }
}
