using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    [SerializeField]
    private List<SkillBase> _skillList = new List<SkillBase>();
    public List<SkillBase> SkillList { get { return _skillList; }}

    public List<SkillBase> ActivatedSkills
    {
        get { return SkillList.Where(skill => skill.IsLearnedSkill).ToList(); }
    }
    
    public Dictionary<SkillType, int> SavedBattleSkill = new Dictionary<SkillType, int>();

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
        GameObject prefab = ResourceManager.Instance.Load<GameObject>($"{prefabName}");
        GameObject go = PoolManager.Instance.Pop(prefab);
        go.transform.position = position;
        // GameObject go = ResourceManager.Instance.Instantiate(prefabName, pooling: true);
        // Debug.Log(go);

        ProjectileController projectile = go.GetComponent<ProjectileController>();
        //go.transform.position = position;
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
        // 250617_1540 이제 안쓰는듯?
        holyProjectile_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_00_HolyProjectile.prefab");
        holyImpact_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_01_HolyImpact.prefab");
        holyPulse_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_02_HolyPulse.prefab");
        fire00_Prefab = ResourceManager.Instance.Load<GameObject>("Skill_10_Fire00.prefab");


        // 스킬고유 인덱스와 skill을 넣어준다. scriptableObject를 활용했기때문에 prefab만 넣어도 데이터를 추출가능하다.
        // SkillBase skill10001 = holyProjectile_Prefab.GetComponent<SkillBase>();
        // SkillBase skill10011 = holyImpact_Prefab.GetComponent<SkillBase>();
        // SkillBase skill10021 = holyPulse_Prefab.GetComponent<SkillBase>();
        // SkillBase skill10031 = fire00_Prefab.GetComponent<SkillBase>();

        _objects.Add("10001", holyProjectile_Prefab);
        _objects.Add("10011", holyImpact_Prefab);
        _objects.Add("10021", holyPulse_Prefab);
        _objects.Add("10031", fire00_Prefab);

        // 이렇게 추가하는 걸 아래처럼 직접하는 게 아니라, AddSkill()메소드로 뺄것 : SkillBook에 추가
        // _skillList.Add(skill10001);
        // _skillList.Add(skill10011);
        // _skillList.Add(skill10021);
        // _skillList.Add(skill10031);
    }

    void Start()
    {
        ResourceManager.Instance.LoadAllAsync<GameObject>("UI_Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                StartSkillUILoad();
            }
        });
    }

    public GameObject skillSelectPopup_Prefab;

    public void StartSkillUILoad()
    {
        skillSelectPopup_Prefab = ResourceManager.Instance.Load<GameObject>("UI_SkillCardSelectPopup");
    }

    void Update()
    {

    }
}
