using UnityEngine;

public class MonsterFinalBoss : MonsterBase
{
    [Header("최종보스 페이즈 연출")]
    [SerializeField] private GameObject phase2Effect;
    [SerializeField] private GameObject phase3Effect;
    [SerializeField] private AudioClip phaseChangeSound;

    public float HPPercent => (float)currentHP / monsterData.maxHP;

    protected override void Update()
    {
        base.Update();
    }

    public void EnterPhase(int phase)
    {
        switch (phase)
        {
            case 2:
                monsterData.attackPower += 15;
                monsterData.moveSpeed += 1;
                if (phase2Effect != null)
                    Instantiate(phase2Effect, transform.position, Quaternion.identity);
                break;

            case 3:
                monsterData.attackPower += 20;
                monsterData.moveSpeed += 1;
                if (phase3Effect != null)
                    Instantiate(phase3Effect, transform.position, Quaternion.identity);
                break;
        }
    }

    public override void ResetMonster()
    {
        base.ResetMonster();    // HP, 애니메이션, 상태머신 등 공통 리셋
    }

    public override void OnDeadEnd()
    {
        base.OnDeadEnd();
        StageManager.Instance.StageClear();
    }
}
