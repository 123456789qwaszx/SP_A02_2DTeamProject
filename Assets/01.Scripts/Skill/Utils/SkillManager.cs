using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    #region HolyProjectile
    public GameObject holyProjectile_Prefab;

    public ProjectileController SpawnHolyProjectile(Vector2 position)
    {
        GameObject go = PoolManager.Instance.Pop(holyProjectile_Prefab);
        go.transform.position = position;

        ProjectileController sc = go.GetComponent<ProjectileController>();
        Projectiles.Add(sc);
        sc.Init();

        return sc;
    }

    public void DespawnHolyProJectile(ProjectileController go)
    {
        Projectiles.Remove(go);
        Instance.SpawnHolyImpact(go.transform.position);

        PoolManager.Instance.Push(go.gameObject);

    }
    #endregion

    #region HolyImpact
    public GameObject holyImpact_Prefab;

    public ProjectileController SpawnHolyImpact(Vector2 position)
    {
        GameObject go = PoolManager.Instance.Pop(holyImpact_Prefab);
        go.transform.position = position;

        ProjectileController sc = go.GetComponent<ProjectileController>();
        sc.Init();

        return sc;
    }

    public void DeSpawnHolyImpact(ProjectileController go)
    {
        PoolManager.Instance.Push(go.gameObject);
    }
    #endregion


    #region HolyPulse
    public GameObject holyPulse_Prefab;

    public ProjectileController SpawnHolyPulse(Vector2 position)
    {
        GameObject go = PoolManager.Instance.Pop(holyPulse_Prefab);
        go.transform.position = position;

        ProjectileController sc = go.GetComponent<ProjectileController>();
        sc.Init();

        return sc;
    }

    public void DeSpawnHolyPulse(ProjectileController go)
    {
        Projectiles.Remove(go);
        PoolManager.Instance.Push(go.gameObject);
    }
    #endregion

    #region SkillData
    public Dictionary<string, Object> _objects = new Dictionary<string, Object>();

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

    public ProjectileController SpawnProjectile(Vector3 position, string prefabName = "")
    {
        GameObject prefab = Load<GameObject>($"{prefabName}");
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


    GameObject fire00_Prefab;

    SkillBase iceSkill_Prefab;

    public void StartSkillLoad()
    {
        holyProjectile_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_00_HolyProjectile.prefab");
        holyImpact_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_01_HolyImpact.prefab");
        holyPulse_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_02_HolyPulse.prefab");
        fire00_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_10_Fire00.prefab");

        

        _objects.Add("0", holyProjectile_Prefab);
        _objects.Add("1", holyImpact_Prefab);
        _objects.Add("2", holyPulse_Prefab);
        _objects.Add("3", fire00_Prefab);
    }

    void Update()
    {
    }
    

}
