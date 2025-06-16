using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCard : MonoBehaviour
{
    // 나중에 플레이어에 SkillBook이 들어가면 그걸 사용할 것
    public SkillBook skillbook;

    // 스킬 Name
    // 스킬 Description

    private SkillBase _skill;

    //string description
    //string SkillName;

    public void SetInfo(SkillBase skill)
    {
        //0. 스킬입력
        _skill = skill;

        //1. 각각 세팅
        // - description = skill.SkillData.Description;
        // - SkillName = skill.SkillData.Name;

        // 별세팅
        // GetImage((int)Images.StarOn_1).gameObject.SetActive(_skill.Level + 1 >= 2);
    }

    public void OnClicked()
    {
        skillbook.LevelUpSkill(_skill.SkillType);
        Debug.Log(_skill.SkillType);
        Debug.Log(SkillManager.Instance.SavedBattleSkill[_skill.SkillType]);
        // UI 닫기
    }
}
