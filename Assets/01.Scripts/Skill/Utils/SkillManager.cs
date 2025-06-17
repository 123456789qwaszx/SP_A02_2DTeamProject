using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    #region HolyProjectile
    public void DespawnHolyProJectile(ProjectileController go)
    {
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


    public GameObject holyProjectile_Prefab;
    public GameObject holyPulse_Prefab;
    GameObject fire00_Prefab;

    // 스킬북에서 로드 중
    // 이 부분이 틀려지면 데이터를 다른 곳에서 받아옴.
    // 아마 _object의 Value를 프리팹으로 넣는 대신에, 그냥 저 프리팹의 데이터를 밸류로 넣어주자고...
    public void StartSkillLoad()
    {
        _objects.Add("10001", holyProjectile_Prefab);
        _objects.Add("10011", holyImpact_Prefab);
        _objects.Add("10021", holyPulse_Prefab);
        _objects.Add("10031", fire00_Prefab);
    }

    // 나중에 UIManager 추가되면 그곳으로 이동
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
}
