using System;
using System.Collections;
using System.Collections.Generic;
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

    #region SkillData
    Dictionary<string, Object> _objects = new Dictionary<string, Object>();

    public T Load<T>(string key) where T : Object
    {
        if (_objects.TryGetValue(key, out Object obj))
        {
            return obj as T;
        }
        return null;
    }
    #endregion

    #region 투사체 통합예정
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

    public ProjectileController SpawnProjectile(Vector3 position, string key)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        GameObject go = PoolManager.Instance.Pop(prefab);
        go.transform.position = position;

        ProjectileController projectile = go.GetComponent<ProjectileController>();
        Projectiles.Add(projectile);

        return projectile;
    }

    public void DespawnProjectile(ProjectileController Skill)
    {
        // 모든 투사체를 관리하는 하나의 해쉬셋 준비할 것.
        PoolManager.Instance.Push(Skill.gameObject);
    }
    #endregion
    
}