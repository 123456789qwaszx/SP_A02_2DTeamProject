using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCard : MonoBehaviour
{
    private SkillBase _skill;

    public TextMeshProUGUI _skillName;
    public TextMeshProUGUI _skillDescription;
    public TextMeshProUGUI _skillCurLevel;
    public TextMeshProUGUI _skillNextLevel;
    public Image _skillIcon;

    public Image _starOn_1;
    public Image _starOn_2;
    public Image _starOn_3;
    public Image _starOn_4;
    public Image _starOn_5;


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
        _skillIcon.sprite = _skill.SkillData.Icon;

        // 별세팅 만약 6개가 꽉찬 상태라면 셀렉트창에 나오지 않기때문에 0은 제외함.
        _starOn_1.gameObject.SetActive(_skill.Level + 1 >= 2);
        _starOn_2.gameObject.SetActive(_skill.Level + 1 >= 3);
        _starOn_3.gameObject.SetActive(_skill.Level + 1 >= 4);
        _starOn_4.gameObject.SetActive(_skill.Level + 1 >= 5);
        _starOn_5.gameObject.SetActive(_skill.Level + 1 >= 6);

    }

    public void OnClicked()
    {
        GameManager.Instance.controller.sklilbook.LevelUpSkill(_skill.SkillType);
        // UI 닫기
        //ResourceManager.Instance.Destroy(gameObject);
    }
}
