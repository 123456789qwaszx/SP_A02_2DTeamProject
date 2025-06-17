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
    public Dictionary<string, SkillData> _skillData = new Dictionary<string, SkillData>();

    public T LoadSkillData<T>(string key) where T : SkillData
    {
        if (_skillData.TryGetValue(key, out SkillData skillData))
        {
            return skillData as T;
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


    // 스킬북에서 로드 중
    // 이 부분을 오타내면 이상한 데이터를 받음.
    // 아마 _object의 Value를 프리팹으로 넣는 대신에, 그냥 저 프리팹의 데이터를 밸류로 넣어주자고...
    public void StartSkillLoad()
    {
        // 지금은 하드코딩 했는데, 데이터가 있으면 바꿀 수 있을 듯.
        // 직업별 스킬 데이터를 가지고 온다음,
        // 그 데이터의 이름을 Load하는 식으로...
        // 또 앞의 변수도 이렇게 하나하나 쓰는게 아니라 +10을 하는식으로 할 수도 있는데 나중에 데이터 생기면 완성하자
        GameObject s_10001 = ResourceManager.Instance.Load<GameObject>("HolyProjectile");
        GameObject s_10011 = ResourceManager.Instance.Load<GameObject>("HolyPulse");
        GameObject s_10021 = ResourceManager.Instance.Load<GameObject>("DarkArrow");
        GameObject s_10031 = ResourceManager.Instance.Load<GameObject>("DirtySwing");
        GameObject s_10041 = ResourceManager.Instance.Load<GameObject>("DirtyHalo");
        GameObject s_10051 = ResourceManager.Instance.Load<GameObject>("DirtyWing");
        GameObject s_10061 = ResourceManager.Instance.Load<GameObject>("PoisionBomb");
        GameObject s_10071 = ResourceManager.Instance.Load<GameObject>("WindCutter");
        GameObject s_10081 = ResourceManager.Instance.Load<GameObject>("Waterfall");
        GameObject s_10091 = ResourceManager.Instance.Load<GameObject>("FireExplosion");
        GameObject s_10101 = ResourceManager.Instance.Load<GameObject>("FireSwing");
        GameObject s_10111 = ResourceManager.Instance.Load<GameObject>("BloodChain");

        SkillBase _10001 = s_10001.GetComponent<SkillBase>();
        SkillBase _10011 = s_10011.GetComponent<SkillBase>();
        SkillBase _10021 = s_10021.GetComponent<SkillBase>();
        SkillBase _10031 = s_10031.GetComponent<SkillBase>();
        SkillBase _10041 = s_10041.GetComponent<SkillBase>();
        SkillBase _10051 = s_10051.GetComponent<SkillBase>();
        SkillBase _10061 = s_10061.GetComponent<SkillBase>();
        SkillBase _10071 = s_10071.GetComponent<SkillBase>();
        SkillBase _10081 = s_10081.GetComponent<SkillBase>();
        SkillBase _10091 = s_10091.GetComponent<SkillBase>();
        SkillBase _10101 = s_10101.GetComponent<SkillBase>();
        SkillBase _10111 = s_10111.GetComponent<SkillBase>();

        _skillData.Add("10001", _10001.SkillData);
        _skillData.Add("10011", _10011.SkillData);
        _skillData.Add("10021", _10021.SkillData);
        _skillData.Add("10031", _10031.SkillData);
        _skillData.Add("10041", _10041.SkillData);
        _skillData.Add("10051", _10051.SkillData);
        _skillData.Add("10061", _10061.SkillData);
        _skillData.Add("10071", _10071.SkillData);
        _skillData.Add("10081", _10081.SkillData);
        _skillData.Add("10091", _10091.SkillData);
        _skillData.Add("10101", _10101.SkillData);
        _skillData.Add("10111", _10111.SkillData);
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
