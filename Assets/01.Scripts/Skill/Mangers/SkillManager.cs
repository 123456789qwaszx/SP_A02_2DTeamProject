using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField]
    private List<SkillBase> _skillList = new List<SkillBase>();
    public List<SkillBase> SkillList { get { return _skillList; } }

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



    // 나중에 UIManager 추가되면 그곳으로 이동
    public void LoadSkill()
    {
        ResourceManager.Instance.LoadAllAsync<GameObject>("Skill_Prefabs", (key, count, totalCount) =>
        {
            //Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                SkillType HolyProjectile = SkillManager.Instance.GetSkillTypeFromInt(10001);
                SkillType HolyPulse = SkillManager.Instance.GetSkillTypeFromInt(10011);
                SkillType DarkArrow = SkillManager.Instance.GetSkillTypeFromInt(10021);
                //SkillType WindCutter = SkillManager.Instance.GetSkillTypeFromInt(10071);
                SkillType BloodChain = SkillManager.Instance.GetSkillTypeFromInt(10111);


                // 처음 무조건 들고 있는 스킬들.
                // 직업 추가 등의 이유로 바꾸거나, 종류를 늘려주고 싶다면
                // 뒤의 SkillIndex를 바꾸거나 추가로 AddSkill()을 할 것.
                GameManager.Instance.controller.sklilbook.AddSkill(HolyProjectile, 10001);
                //GameManager.Instance.controller.sklilbook.LevelUpSkill(HolyProjectile);

                GameManager.Instance.controller.sklilbook.AddSkill(HolyPulse, 10011);
                //GameManager.Instance.controller.sklilbook.LevelUpSkill(HolyPulse);

                GameManager.Instance.controller.sklilbook.AddSkill(DarkArrow, 10021);
                //GameManager.Instance.controller.sklilbook.LevelUpSkill(DarkArrow);

                //GameManager.Instance.controller.sklilbook.AddSkill(WindCutter, 10071);
                //GameManager.Instance.controller.sklilbook.LevelUpSkill(WindCutter);

                GameManager.Instance.controller.sklilbook.AddSkill(BloodChain, 10111);
                //GameManager.Instance.controller.sklilbook.LevelUpSkill(BloodChain);

                GameManager.Instance.controller.sklilbook.AddSkill(GetSkillTypeFromInt(10121), 10121);
                GameManager.Instance.controller.sklilbook.LevelUpSkill(GetSkillTypeFromInt(10121));
                // 이렇게 직접 레벨업 시키는 건 처음 시작시 세팅만 이렇고, 이후는 스킬카드UI의 버튼을 통해 LevelUpSkill이 실행됨

            }
        });

        ResourceManager.Instance.LoadAllAsync<GameObject>("UI_Prefabs", (key, count, totalCount) =>
        {
            //Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                StartSkillUILoad();
            }
        });
        
        // ResourceManager.Instance.LoadAllAsync<GameObject>("default", (key, count, totalCount) =>
        // {
        //     Debug.Log($"{key} {count}/{totalCount}");

        //     if (count == totalCount)
        //     {
        //         GameObject go = ResourceManager.Instance.Load<GameObject>("Player_Warrior");
        //         GameManager.Instance.player = go.GetComponent<Player>();
        //     }
        // });
    }

    public GameObject skillSelectPopup_Prefab;

    public void StartSkillUILoad()
    {
        skillSelectPopup_Prefab = ResourceManager.Instance.Load<GameObject>("UI_SkillCardSelectPopup");
    }
    
    
    public SkillType GetSkillTypeFromInt(int value)
    {
        foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
        {
            int minValue = (int)skillType;
            int maxValue = minValue + 5; // 100501~ 100506 사이 값이면 100501값 리턴

            if (value >= minValue && value <= maxValue)
            {
                return skillType;
            }
        }

        Debug.LogError($" Faild add skill : {value}");
        return SkillType.None;
    }
}
