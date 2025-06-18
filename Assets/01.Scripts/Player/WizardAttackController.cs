using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wizard))]
public class WizardAttackController : MonoBehaviour
{
    [Header("공격 범위 설정")]
    public float attackRange = 5f;

    [Header("시각화 설정")]
    public float showTime = 0.3f;              // 공격 효과 유지 시간 (초)
    public float attackCircleSize = 0.7f;      // 생성될 원 Sprite의 반지름 및 데미지 적용 영역

    [Header("공격 조건")]
    public float allowedAngle = 20f;           // 클릭 방향과 허용되는 각도 범위

    [Header("몬스터 레이어")]
    public LayerMask monsterLayer;             // 공격 대상 레이어

    private Wizard wizard;

    void Awake()
    {
        wizard = GetComponent<Wizard>();
    }

    public void CastSpell(Vector3 mouseWorldPos)
    {
        Vector3 attackOrigin = transform.position;
        Vector3 computedDir = (mouseWorldPos - attackOrigin).normalized;

        CreateStaticAttackEffect(attackOrigin, computedDir);
    }

    private void CreateStaticAttackEffect(Vector3 origin, Vector3 direction)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, monsterLayer);
        Transform target = null;
        float minDist = Mathf.Infinity;

        foreach (Collider2D col in hits)
        {
            Vector3 toCandidate = col.transform.position - origin;
            float angleDiff = Vector3.Angle(direction, toCandidate);
            if (angleDiff <= allowedAngle)
            {
                float dist = toCandidate.magnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    target = col.transform;
                }
            }
        }

        Vector3 effectCenter = (target != null) ? target.position : origin + direction * attackRange;

        // Sprite 기반 시각화로 변경
        GameObject effectGo = new GameObject("AttackEffect");
        effectGo.transform.position = effectCenter;
        effectGo.transform.rotation = Quaternion.identity;
        effectGo.tag = "AttackEffect";

        SpriteRenderer sr = effectGo.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("CircleSprite"); // Resources 폴더 안에 CircleSprite가 있어야 함
        sr.color = new Color(1f, 0f, 0f, 0.5f);              // 반투명 빨간색
        effectGo.transform.localScale = new Vector3(attackCircleSize * 2, attackCircleSize * 2, 1);

        // 데미지 적용
        float finalDamage = wizard.GetDamage();
        Collider2D[] damageHits = Physics2D.OverlapCircleAll(effectCenter, attackCircleSize, monsterLayer);
        foreach (Collider2D col in damageHits)
        {
            IDamagable damagable = col.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(finalDamage);
                Debug.Log("Damage applied to " + col.gameObject.name);
            }
        }

        Destroy(effectGo, showTime);
    }
}