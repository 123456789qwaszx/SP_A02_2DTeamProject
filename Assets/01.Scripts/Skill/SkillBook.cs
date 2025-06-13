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


    public void AddSkill(SkillType skillType, int skillId = 0)
    {
        string className = skillType.ToString();

        RepeatSkill skillBase = TestSkill.Instance.fireSkill_Prefab;//gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
            Debug.Log(skillBase);
        GameManager.Instance.SkillList.Add(skillBase);
        if (GameManager.Instance.SavedBattleSkill.ContainsKey(skillType))
            GameManager.Instance.SavedBattleSkill[skillType] = skillBase.Level;
        else
            GameManager.Instance.SavedBattleSkill.Add(skillType, skillBase.Level);
    }


    public void AddActivatedSkills(SkillBase skill)
    {
        GameManager.Instance.ActivatedSkills.Add(skill);
    }

    public void LevelUpSkill(SkillType skillType)
    {
        for (int i = 0; i < GameManager.Instance.SkillList.Count; i++)
        {
            if (GameManager.Instance.SkillList[i].SkillType == skillType)
            {
                GameManager.Instance.SkillList[i].OnLevelUp();
                if (GameManager.Instance.SavedBattleSkill.ContainsKey(skillType))
                {
                    GameManager.Instance.SavedBattleSkill[skillType] = GameManager.Instance.SkillList[i].Level;
                }
            }
        }
    }


    public void Clear()
    {
        GameManager.Instance.SavedBattleSkill.Clear();
    }



    #region Refactor예정
    [Header("HolyProjectile")]
    public Transform indicator;
    public float _projectileCooldown = 0.3f;

    void Start()
    {
        StartProjectile();
        StartPulse();
        AddSkill(SkillType.FireSkill, 1);
        AddSkill(SkillType.FireSkill, 2);
    }


    #region HolyProjectile
    Coroutine _coProjectile;

    void StartProjectile()
    {
        if (_coProjectile != null)
            StopCoroutine(_coProjectile);

        _coProjectile = StartCoroutine(CoStartProjectile());
    }

    IEnumerator CoStartProjectile()
    {
        new WaitForSeconds(_projectileCooldown);

        while (true)
        {
            Vector2 dir = -(transform.position - indicator.position).normalized;

            SkillController skill = SkillManager.Instance.SpawnHolyProjectile(transform.position);
            skill.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            skill.SetInfo(GameManager.Instance.player, -(transform.position - indicator.position).normalized, skill.lifeTime, skill.damage);
            yield return new WaitForSeconds(_projectileCooldown);
        }
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
        new WaitForSeconds(_projectileCooldown);

        while (true)
        {
            SkillController skill = SkillManager.Instance.SpawnHolyPulse(transform.position);

            skill.SetInfo(GameManager.Instance.player, GameManager.Instance.MoveDir, skill.lifeTime = 2f, skill.damage);
            yield return new WaitForSeconds(_projectileCooldown);
        }
    }
    #endregion
    #endregion
}
