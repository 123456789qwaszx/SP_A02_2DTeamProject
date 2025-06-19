using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Meteor : RepeatSkill
{
    private void Awake()
    {
        SkillType = SkillType.Meteor;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }

    IEnumerator GenerateMeteor()
    {
        List<MonsterBase> targets = ObjectManager.Instance.GetMonsterWithinCamera(SkillData.skillLevel[Level].projectileCount);
        
        if (targets == null)
            yield break;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].IsValid() == true)
            { 
                Vector2 startPos = GetMeteorPositgion(targets[i].transform.position);
                GenerateProjectile(GameManager.Instance.controller, "Meteor", startPos, Vector3.zero, targets[i].transform.position, this, Level);
                yield return new WaitForSeconds(SkillData.skillLevel[Level].AttackInterval);
            }
        }
    }

    public Vector2 GetMeteorPositgion(Vector3 target)
    {
        float angleInRadians = 60f * Mathf.Deg2Rad;
        float spawnMargin = 1f;
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        float spawnX = target.x + (halfWidth + spawnMargin) * Mathf.Cos(angleInRadians);
        float spawnY = target.y + (halfHeight + spawnMargin) * Mathf.Sin(angleInRadians);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        return spawnPosition;
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(GenerateMeteor());
    }
}
