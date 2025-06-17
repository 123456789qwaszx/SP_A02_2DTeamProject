using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCard : MonoBehaviour
{

    // 스킬 Name
    // 스킬 Description

    private SkillBase _skill;

    public TextMeshProUGUI _skillName;
    public TextMeshProUGUI _skillDescription;
    public TextMeshProUGUI _skillCurLevel;
    public TextMeshProUGUI _skillNextLevel;
    public Image _skillIcon;

    //string description
    //string SkillName;

    public void SetInfo(SkillBase skill)
    {
        //0. 스킬입력
        _skill = skill;

        //1. 각각 세팅
        // - description = skill.SkillData.Description;
        // - SkillName = skill.SkillData.Name;
        _skillName.text = _skill.SkillData.PrefabLabel;
        _skillDescription.text = _skill.SkillData.Description;
        _skillCurLevel.text = $"{SkillManager.Instance.SavedBattleSkill[_skill.SkillType]}";
        _skillNextLevel.text = $"{SkillManager.Instance.SavedBattleSkill[_skill.SkillType] + 1}";
        //_skillIcon = GetComponent<Image>();
        _skillIcon.sprite = _skill.SkillData.Icon;


        // 별세팅
        // GetImage((int)Images.StarOn_1).gameObject.SetActive(_skill.Level + 1 >= 2);
    }

    public void OnClicked()
    {
        GameManager.Instance.controller.sklilbook.LevelUpSkill(_skill.SkillType);
        Debug.Log(_skill.SkillType);
        Debug.Log(SkillManager.Instance.SavedBattleSkill[_skill.SkillType]);
        // UI 닫기
    }
}
