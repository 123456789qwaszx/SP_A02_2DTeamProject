using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill :Singleton<TestSkill>
{
    public FireSkill fireSkill_Prefab;
    public SkillBase iceSkill_Prefab;

    void Awake()
    {
        SkillManager.Instance._objects.Add("1", fireSkill_Prefab);
        SkillManager.Instance._objects.Add("2", iceSkill_Prefab);
    }

}
