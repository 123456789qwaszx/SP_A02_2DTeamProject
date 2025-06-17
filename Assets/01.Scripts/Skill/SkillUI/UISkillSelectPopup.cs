using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISkillSelectPopup : MonoBehaviour
{
    // Exp Point가 Max일 경우, ResourceManager를 통해 켜짐.
    // 켜지면서 자동으로 Init
    private void OnEnable()
    {
        Init();

    }

    public UI_SkillCard item1;
    public UI_SkillCard item2;
    public UI_SkillCard item3;




    public void Init()
    {
        // 각 스킬카드에 Skill을 기입
        SetReCommendSkills();
    }

    void SetReCommendSkills()
    {
        // 총 3개의 랜덤한 스킬을 가져옴
        List<SkillBase> List = GameManager.Instance.controller.sklilbook.RecommendSkills();
        Debug.Log(List.Count);

        // 나중엔 (List[1]), (List[2])로 바꿀것
        Debug.Log(List.Count);
        item1.GetComponent<UI_SkillCard>().SetInfo(List[0]);
        item2.GetComponent<UI_SkillCard>().SetInfo(List[1]);
        item3.GetComponent<UI_SkillCard>().SetInfo(List[2]);
    }
}
