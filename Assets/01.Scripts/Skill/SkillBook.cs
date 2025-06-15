using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    [SerializeField]
    private List<SkillBase> _skillList = new List<SkillBase>();
    public List<SkillBase> SkillList { get { return _skillList; }}

    public List<SkillBase> ActivatedSkills
    {
        get { return SkillList.Where(skill => skill.IsLearnedSkill).ToList(); }
    }
    
    public Dictionary<SkillType, int> SavedBattleSkill = new Dictionary<SkillType, int>();

    public void LoadSkill(SkillType skillType, int level)
    {
        AddSkill(skillType);
        for (int i = 0; i < level; i++)
        {
            LevelUpSkill(skillType);
        }
    }
    
    
    void Start()
    {
        ResourceManager.Instance.LoadAllAsync<GameObject>("Skill_Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                SkillManager.Instance.StartSkillLoad();

                StartProjectile();
    
            }
        });
    }


    public void AddSkill(SkillType skillType, int skillId = 0)
    {
        string className = skillType.ToString();

        RepeatSkill skillBase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
        Debug.Log(skillBase);
        SkillList.Add(skillBase);
        if (SavedBattleSkill.ContainsKey(skillType))
            SavedBattleSkill[skillType] = skillBase.Level;
        else
            SavedBattleSkill.Add(skillType, skillBase.Level);
    }


    public void AddActivatedSkills(SkillBase skill)
    {
        ActivatedSkills.Add(skill);
    }

    public void LevelUpSkill(SkillType skillType)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].SkillType == skillType)
            {
                SkillList[i].OnLevelUp();
                if (SavedBattleSkill.ContainsKey(skillType))
                {
                    SavedBattleSkill[skillType] = SkillList[i].Level;
                }
            }
        }
    }


    public void Clear()
    {
        SavedBattleSkill.Clear();
    }



    #region Refactor예정
    [Header("HolyProjectile")]
    public Transform indicator;


    #region HolyProjectile
    Coroutine _coProjectile;

    public void StartProjectile()
    {
        if (_coProjectile != null)
            StopCoroutine(_coProjectile);

        _coProjectile = StartCoroutine(CoStartProjectile());
    }

    IEnumerator CoStartProjectile()
    {
        do
        {
            Vector2 dir = -(transform.position - indicator.position).normalized;

            ProjectileController skill = SkillManager.Instance.SpawnHolyProjectile(transform.position);
            skill.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            Debug.Log(skill);

            skill.SetInfo(GameManager.Instance.controller, GameManager.Instance.controller.transform.position, -(transform.position - indicator.position).normalized, indicator.transform.position, SkillManager.Instance.holyProjectile_Prefab);
            yield return new WaitForSeconds(skill._attackInterval);
        }

        while (true);
    }
    #endregion

    #region HolyPulse
    Coroutine _coPulse;

    void StartPulse()
    {
        if (_coPulse != null)
            StopCoroutine(_coPulse);

            _coPulse = StartCoroutine(CoStartPulse());
    }

    IEnumerator CoStartPulse()
    {
        do

        {
            ProjectileController skill = SkillManager.Instance.SpawnHolyPulse(transform.position);

            skill.SetInfo(GameManager.Instance.controller, GameManager.Instance.controller.transform.position, -(transform.position - indicator.position).normalized, indicator.transform.position, SkillManager.Instance.holyPulse_Prefab);
            yield return new WaitForSeconds(skill._attackInterval);
        }

        while (true);
    }
    #endregion
    #endregion
}
