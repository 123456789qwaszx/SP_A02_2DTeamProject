using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public SkillBook Skills { get; set; }

    void Start()
    {
        Skills = gameObject.GetOrAddComponent<SkillBook>();

        InitSkill();
    }

    void Update()
    {

    }


    public void InitSkill()
    {
        //만약 가능하다면 SkillType startSkill = abc 이런식으로 추가하는 것도 좋을 듯.

        //Skills.AddSkill(Skills.);
        //각자 직업에 맞는 기본 스킬 추가
        //Skills.AddSkill(SkillType.HolyProjectile, 1);

        //기본 0(비활성화)에서 1레벨로 활성화
        //Skills.LevelUpSkill(SkillType.HolyProjectile);
    }

}
