using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    
    public static int MAX_SKILL_LEVEL = 6;
    public static int MAX_SKILL_COUNT = 6;

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

                SkillType type = Util.GetSkillTypeFromInt(10001);

                //Test
                //SkillType type2 = Util.GetSkillTypeFromInt(10021);

                if (type != SkillType.None)
                {
                    // 처음 무조건 들고 있는 스킬들.
                    // 직업 추가 등의 이유로 바꾸거나, 종류를 늘려주고 싶다면
                    // 뒤의 SkillIndex를 바꾸거나 추가로 AddSkill()을 할 것.
                    AddSkill(type, 10001);
                    LevelUpSkill(type);
                    // 이렇게 직접 레벨업 시키는 건 처음 시작시 세팅만 이렇고, 이후는 스킬카드UI의 버튼을 통해 LevelUpSkill이 실행됨

                    //Test
                    // AddSkill(type2, 10021);
                    // LevelUpSkill(type2);

                }
            }
        });
    }

    
    public List<SkillBase> RecommendSkills()
    {
        List<SkillBase> skillList = SkillManager.Instance.SkillList.ToList();
        List<SkillBase> activeSkills = skillList.FindAll(skill => skill.IsLearnedSkill);

        if (activeSkills.Count == MAX_SKILL_COUNT)
        {
            List<SkillBase> recommendSkills = activeSkills.FindAll(s => s.Level < MAX_SKILL_LEVEL);
            recommendSkills.Shuffle();
            return recommendSkills.Take(3).ToList();
        }
        else
        {
            List<SkillBase> recommendSkills = skillList.FindAll(s => s.Level < MAX_SKILL_LEVEL);
            recommendSkills.Shuffle();
            return recommendSkills.Take(3).ToList();
        }
    }


    public void AddSkill(SkillType skillType, int skillId = 0)
    {
        // 당장은 무조건 플레이어에 붙여서 반복해서 쏘는 Repeat 스킬만 해당됨.
        // 스킬 종류가 늘어나면 여기서 추가로 분리해줄 것.
        string className = skillType.ToString();
        RepeatSkill skillBase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
        SkillManager.Instance.SkillList.Add(skillBase);
        if (SkillManager.Instance.SavedBattleSkill.ContainsKey(skillType))
            SkillManager.Instance.SavedBattleSkill[skillType] = skillBase.Level;
        else
            SkillManager.Instance.SavedBattleSkill.Add(skillType, skillBase.Level);
    }


    public void AddActivatedSkills(SkillBase skill)
    {
        SkillManager.Instance.ActivatedSkills.Add(skill);
    }
    

    public void LevelUpSkill(SkillType skillType)
    {
        for (int i = 0; i < SkillManager.Instance.SkillList.Count; i++)
        {
            if (SkillManager.Instance.SkillList[i].SkillType == skillType)
            {
                SkillManager.Instance.SkillList[i].OnLevelUp();
                if (SkillManager.Instance.SavedBattleSkill.ContainsKey(skillType))
                {
                    SkillManager.Instance.SavedBattleSkill[skillType] = SkillManager.Instance.SkillList[i].Level;
                }
            }
        }
    }


    public void Clear()
    {
        SkillManager.Instance.SavedBattleSkill.Clear();
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


            skill.SetInfo(GameManager.Instance.controller, GameManager.Instance.controller.transform.position, -(transform.position - indicator.position).normalized, indicator.transform.position, skill);
            yield return new WaitForSeconds(skill._attackInterval);
        }

        while (true);
    }
    #endregion

    #region HolyPulse
    // Coroutine _coPulse;

    // void StartPulse()
    // {
    //     if (_coPulse != null)
    //         StopCoroutine(_coPulse);

    //     _coPulse = StartCoroutine(CoStartPulse());
    // }

    // IEnumerator CoStartPulse()
    // {
    //     do

    //     {
    //         ProjectileController skill = SkillManager.Instance.SpawnHolyPulse(transform.position);

    //         skill.SetInfo(GameManager.Instance.controller, GameManager.Instance.controller.transform.position, -(transform.position - indicator.position).normalized, indicator.transform.position, );
    //         yield return new WaitForSeconds(skill._attackInterval);
    //     }

    //     while (true);
    // }
    #endregion
    #endregion
}