using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

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
}