using UnityEngine;

public class MonsterBoss : MonsterBase
{
    [Header("보스 전용 설정")]
    [SerializeField] private GameObject phaseChangeEffect;
    [SerializeField] private AudioClip phaseChangeSound;

    private bool isPhase2 = false;

    protected override void Update()
    {
        base.Update();

        if (!isPhase2 && currentHP <= monsterData.maxHP * 0.5f)
        {
            EnterPhase2();
        }
    }

    private void EnterPhase2()
    {
        isPhase2 = true;

        monsterData.attackPower += 10;
        monsterData.moveSpeed += 1;

        // 페이즈 전환 이펙트
        //if (phaseChangeEffect != null)
        //    Instantiate(phaseChangeEffect, transform.position, Quaternion.identity);

        //if (phaseChangeSound != null)
        //    AudioManager.Instance.PlaySFX(phaseChangeSound);
    }
    public override void ResetMonster()
    {
        base.ResetMonster();    // HP, 애니메이션, 상태머신 등 공통 리셋
        isPhase2 = false;       // 보스 전용 페이즈 상태 리셋
    }


    public override void OnDeadEnd()
    {
        base.OnDeadEnd();
        StageManager.Instance.StageClear();
    }
}
