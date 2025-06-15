using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillBook : MonoBehaviour
{

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
                Debug.Log(type);

                if (type != SkillType.None)
                {
                    AddSkill(type, 10001);
                    LevelUpSkill(type);
                }

                //StartProjectile();

            }
        });
    }


    public void AddSkill(SkillType skillType, int skillId = 0)
    {
        string className = skillType.ToString();
        Debug.Log(skillType);
        Debug.Log(className);


        RepeatSkill skillBase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
        Debug.Log(skillBase);

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
            Debug.Log(SkillManager.Instance.SkillList.Count);
            Debug.Log(SkillManager.Instance.SkillList[i]);
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