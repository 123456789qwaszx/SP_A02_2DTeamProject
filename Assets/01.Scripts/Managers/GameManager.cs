using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /* PoolManager 사용 예시
    public GameObject bear_Prefab;
    public GameObject SpawnBear() { return PoolManager.Instance.Pop(bear_Prefab); }
    public void DespawnBear(GameObject bear) { PoolManager.Instance.Push(bear); }
    */

    public Player player;
    public PlayerController controller;

    // 16개의 던전 클리어 여부 저장
    // 예: 저장/로드 시스템과 연동하여 stageCleared를 유지
    public int currentStage = 1;
    public int maxUnlockedStage = 1;
    public bool[] stageCleared = new bool[16];

    public void UdateStageInfo()
    {
        int stage = currentStage;
        stageCleared[stage - 1] = true;

        // 다음 스테이지 언락
        if (stage > maxUnlockedStage)
            maxUnlockedStage = stage;

        UIManager.Instance.UpdateStageButtons();
    }

    Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
        }
    }

    #region HolyProjectile
    public GameObject holyProjectile_Prefab;
    public HashSet<SkillController> HolyProjectiles { get; } = new HashSet<SkillController>();

    public SkillController SpawnHolyProjectile(Vector2 position)
    {
        GameObject go = PoolManager.Instance.Pop(holyProjectile_Prefab);
        go.transform.position = position;

        SkillController sc = go.GetComponent<SkillController>();
        HolyProjectiles.Add(sc);
        sc.Init();

        return sc;
    }

    public void DespawnHolyProJectile(SkillController go)
    {
        HolyProjectiles.Remove(go);
        Instance.SpawnHolyImpact(go.transform.position);

        PoolManager.Instance.Push(go.gameObject);

    }
    #endregion

    #region HolyImpact
    public GameObject holyImpact_Prefab;

    public SkillController SpawnHolyImpact(Vector2 position)
    {
        GameObject go = PoolManager.Instance.Pop(holyImpact_Prefab);
        go.transform.position = position;

        SkillController sc = go.GetComponent<SkillController>();
        sc.Init();

        return sc;
    }

    public void DeSpawnHolyImpact(SkillController go)
    {
        PoolManager.Instance.Push(go.gameObject);
    }
    #endregion
    
    
    #region HolyPulse
    public GameObject holyPulse_Prefab;
    public HashSet<SkillController> HolyPulsces { get; } = new HashSet<SkillController>();

    public SkillController SpawnHolyPulse(Vector2 position)
    {
        GameObject go = PoolManager.Instance.Pop(holyPulse_Prefab);
        go.transform.position = position;

        SkillController sc = go.GetComponent<SkillController>();
        sc.Init();

        return sc;
    }

    public void DeSpawnHolyPulse(SkillController go)
    {
        HolyProjectiles.Remove(go);
        PoolManager.Instance.Push(go.gameObject);
    }
    #endregion
}