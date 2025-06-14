using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    [SerializeField]
    private List<SkillBase> _skillList = new List<SkillBase>();
    public List<SkillBase> SkillList { get { return _skillList; }}

    public List<SkillBase> ActivatedSkills
    {
        get { return SkillList.Where(skill => skill.IsLearnedSkill).ToList(); }
    }
    
    public Dictionary<SkillType, int> SavedBattleSkill = new Dictionary<SkillType, int>();

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

        RepeatSkill skillBase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
            Debug.Log(skillBase);
        SkillList.Add(skillBase);
        if (SavedBattleSkill.ContainsKey(skillType))
            SavedBattleSkill[skillType] = skillBase.Level;
        else
            SavedBattleSkill.Add(skillType, skillBase.Level);
    }


    public void AddActivatedSkills(SkillBase skill)
    {
        ActivatedSkills.Add(skill);
    }

    public void LevelUpSkill(SkillType skillType)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].SkillType == skillType)
            {
                SkillList[i].OnLevelUp();
                if (SavedBattleSkill.ContainsKey(skillType))
                {
                    SavedBattleSkill[skillType] = SkillList[i].Level;
                }
            }
        }
    }


    public void Clear()
    {
        SavedBattleSkill.Clear();
    }



    #region Refactor예정
    [Header("HolyProjectile")]
    public Transform indicator;
    public float _projectileCooldown = 0.3f;

    void Start()
    {
        // StartProjectile();
        // StartPulse();
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
