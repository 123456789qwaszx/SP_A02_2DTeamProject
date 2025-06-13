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

    // 5개의 던전 클리어 여부 저장 (0~4번 인덱스)
    public bool[] dungeonCleared = new bool[5];
    public int currentDungeonLevel = 0;


    // 예: 저장/로드 시스템과 연동하여 dungeonCleared를 유지

    Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
        }
    }

    
    [SerializeField]
    private List<SkillBase> _skillList = new List<SkillBase>();
    public List<SkillBase> SkillList { get { return _skillList; }}

    public List<SkillBase> ActivatedSkills
    {
        get { return SkillList.Where(skill => skill.IsLearnedSkill).ToList(); }
    }
    
    public Dictionary<SkillType, int> SavedBattleSkill = new Dictionary<SkillType, int>();
}