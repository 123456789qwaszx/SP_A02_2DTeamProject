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
}