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

    Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
        }
    }

    #region Projectile
    public GameObject projectile_Prefab;
    public HashSet<SkillController> Projectiles { get; } = new HashSet<SkillController>();

    public SkillController SpawnProjectile(Vector3 position)
    {
        GameObject go = PoolManager.Instance.Pop(projectile_Prefab);
        go.transform.position = position;

        SkillController sc = go.GetComponent<SkillController>();
        Projectiles.Add(sc);
        sc.Init();

        return sc;
    }

    public void DespawnProJectile(SkillController go)
    {
        Projectiles.Remove(go);
        PoolManager.Instance.Push(go.gameObject);
    }
    #endregion
}