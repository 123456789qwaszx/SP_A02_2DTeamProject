using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : SkillController
{
    bool _isvalid = false;

    // 게임매니저의 Spawn함수에서 사용됨. Pooling 관련 처리.
    public void InitSwing()
    {
        if (_isvalid)
        {
            _isvalid = false;
        }
        else
        {
            _isvalid = true;
        }
    }

    public void ActivateSwing()
    {
        StartCoroutine(CoSwing());
    }

    float CoolTime = 2.0f;

    IEnumerator CoSwing()
    {
        while (true)
        {
            yield return new WaitForSeconds(CoolTime);

            // 스폰 Holy
        }

    }
}
