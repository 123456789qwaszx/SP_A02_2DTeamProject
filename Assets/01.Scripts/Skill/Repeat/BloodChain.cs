using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodChain : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.BloodChain;
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(CoBloodChain());
    }

    IEnumerator CoBloodChain()
    {
        string prefabName = SkillData.PrefabLabel;

        if (GameManager.Instance.controller != null)
        {
            for (int i = 0; i < SkillData.projectileCount; i++)
            {
                Vector3 startPos = GameManager.Instance.controller.transform.position;
                int minDist = (int)SkillData.BounceDist - 1;
                int maxDist = (int)SkillData.BounceDist + 1;
                List<MonsterBase> targets = GetChainMonsters(SkillData.NumBounce, minDist, maxDist, index : i);
                if (targets == null)
                    continue;
                for (int j = 0; j < targets.Count; j++)
            {
                    if (j > 0)
                        startPos = targets[j - 1].transform.position;
                    Vector3 dir = (targets[j].transform.position - startPos).normalized;
                    GenerateProjectile(GameManager.Instance.controller, prefabName, startPos, dir, targets[j].transform.position, this);
                }
                yield return null;
            }
        }
    }


    public List<MonsterBase> GetChainMonsters(int numTargets, float minDistance, float maxDistance, float angleRange = 180, int index = 0)
    {
        List<MonsterBase> chainMonsters = new List<MonsterBase>();
        // projRange �̻��� ���͸� �˻�
        List<MonsterBase> nearestMonster = ObjectManager.Instance.GetNearestMonsters(SkillData.projectileCount, (int)SkillData.ProjRange);
        if (nearestMonster != null)
        {
            int idx = Mathf.Min(index, nearestMonster.Count-1);
            chainMonsters.Add(nearestMonster[idx]);

            for (int i = 1; i < numTargets; i++)
            {
                MonsterBase chainMonster = GetChainMonster(chainMonsters[i - 1].transform.position, minDistance, maxDistance, angleRange, chainMonsters);
                if (chainMonster != null)
                {
                    chainMonsters.Add(chainMonster);
                }
                else
                {
                    break;
                }
            }
        }

        return chainMonsters;
    }

    public MonsterBase GetChainMonster(Vector3 origin, float minDistance, float maxDistance, float angleRange, List<MonsterBase> ignoreMonsters)
    {
        LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
        Collider2D[] targets = Physics2D.OverlapCircleAll(origin, maxDistance, targetLayer);

        float closestDistance = Mathf.Infinity;
        MonsterBase closestMonster = null;
        foreach (Collider2D target in targets)
        {
            if (ignoreMonsters.Contains(target.GetComponent<MonsterBase>()))
            {
                continue;
            }

            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(origin, targetPosition);
            if (distance >= minDistance && distance <= maxDistance)
            {
                Vector3 direction = (targetPosition - origin).normalized;
                float angle = Vector3.Angle(direction, Vector3.up);
                //if (angle < angleRange / 2f)
                {
                    closestDistance = distance;
                    closestMonster = target.GetComponent<MonsterBase>();
                }
            }
        }

        return closestMonster;
    }
}
