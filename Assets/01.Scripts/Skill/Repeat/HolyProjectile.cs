using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyProjectile : RepeatSkill
{
    //public SkillData _skillData;
    private void Awake()
    {
        SkillType = SkillType.HolyProjectile;
    }

    // private void Start()
    // {
    //     Debug.Log("수정필요1");
    //     // 당장은 이렇게 넣는데, 자연스럽게 SetActive(true)가 되면서 ActiveSkill이 되도록 할 것
    //     ActivateSkill();
    // }


    protected override void DoSkillJob()
    {
        StartCoroutine(SetHolyProjectile());
        Debug.Log("스킬 실시q");
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        StartCoroutine(SetHolyProjectile());
        //gameObject.SetActive(true);
    }
    IEnumerator SetHolyProjectile()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        // 아이템 데이터를 올바르게 바꿔야할 수도...
        string prefabName = SkillType.ToString();
        Vector3 position = GameManager.Instance.controller.transform.position;
        while (true)
        {
            Debug.Log(SkillData);
        for (int i = 0; i < SkillData.projectileCount; i++)
            {
                Vector3 dir = -(position - indicator.transform.position).normalized;

                GenerateProjectile(GameManager.Instance.controller, prefabName, position, dir, indicator.transform.position, this);

                yield return wait;//new WaitForSeconds(SkillData.AttackInterval);

            }
        }
    }
}


    // protected virtual void GenerateProjectile(PlayerController Owner, string prefabName, Vector3 startPos, Vector3 dir, Vector3 targetPos, GameObject skill_prefab)
    // {
    //     ProjectileController pc = SkillManager.Instance.SpawnProjectile(startPos, prefabName: prefabName);
    //     pc.SetInfo(Owner, startPos, dir, targetPos, skill_prefab);
    // }


// 처음 만들었던 것 참고용
    // #region HolyProjectile
    // Coroutine _coProjectile;

    // public void StartProjectile()
    // {
    //     if (_coProjectile != null)
    //         StopCoroutine(_coProjectile);

    //     _coProjectile = StartCoroutine(CoStartProjectile());
    // }

    // IEnumerator CoStartProjectile()
    // {
    //     do
    //     {
    //         Vector2 dir = -(transform.position - indicator.position).normalized;

    //         ProjectileController skill = SkillManager.Instance.SpawnHolyProjectile(transform.position);
    //         skill.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);


    //         skill.SetInfo(GameManager.Instance.controller, GameManager.Instance.controller.transform.position, -(transform.position - indicator.position).normalized, indicator.transform.position, SkillManager.Instance.holyProjectile_Prefab);
    //         yield return new WaitForSeconds(skill._attackInterval);
    //     }

    //     while (true);
    // }
    // #endregion
