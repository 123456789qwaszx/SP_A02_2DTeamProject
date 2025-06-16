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
    public float attackCircleSize = 0.7f;        // 생성될 원 Mesh의 반지름 및 데미지 적용 영역

    [Header("공격 조건")]
    public float allowedAngle = 20f;           // 클릭 방향과 허용되는 각도 범위

    [Header("몬스터 레이어")]
    public LayerMask monsterLayer;           // 공격 대상 레이어

    private Wizard wizard;

    void Awake()
    {
        wizard = GetComponent<Wizard>();
    }

    /// <summary>
    /// 마우스 클릭을 받아 공격을 실행
    /// 공격 효과 및 데미지 처리를 진행
    /// </summary>
    /// <param name="mouseWorldPos">Camera.ScreenToWorldPoint()로 얻은 월드 좌표 (z=0)</param>
    public void CastSpell(Vector3 mouseWorldPos)
    {
        // 공격 시점의 플레이어 위치
        Vector3 attackOrigin = transform.position;
        // 클릭에서 기본 방향(정규화 벡터) 계산
        Vector3 computedDir = (mouseWorldPos - attackOrigin).normalized;

        // 공격 효과 및 데미지 처리
        CreateStaticAttackEffect(attackOrigin, computedDir);
    }

    /// <summary>
    /// 공격 시점의 위치와 보정된 클릭 방향에 따라,
    /// attackRange 내에서 클릭 방향과의 각도가 allowedAngle 이하인 대상 중 플레이어 기준 가장 가까운 몬스터를 찾음
    /// 해당 타깃이 있으면 그 위치에, 없으면 클릭 방향의 사거리 끝에 생성
    /// 효과 영역 내에 존재하는 몬스터에게 Wizard의 데미지를 적용
    /// 생성된 효과는 showTime 초 후 자동 삭제
    /// </summary>
    /// <param name="origin">공격 시작 시점의 플레이어 위치</param>
    /// <param name="direction">보정된 클릭 방향 (정규화된 벡터)</param>
    private void CreateStaticAttackEffect(Vector3 origin, Vector3 direction)
    {
        // 공격 범위 내의 모든 몬스터 검색 (monsterLayer 사용)
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, monsterLayer);
        Transform target = null;
        float minDist = Mathf.Infinity;

        // 클릭 방향과의 각도가 allowedAngle 이하인 대상 중에서 플레이어에 가장 가까운 몬스터 선택
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

        // 효과 생성 위치: target이 있으면 target.position, 없으면 클릭 방향의 끝 (플레이어 위치 + attackRange)
        Vector3 effectCenter = (target != null) ? target.position : origin + direction * attackRange;

        // 공격 효과 GameObject 생성
        GameObject effectGo = new GameObject("AttackEffect");
        effectGo.transform.position = effectCenter;
        effectGo.transform.rotation = Quaternion.identity;

        // Mesh 컴포넌트 추가 및 재질 설정 (반투명 빨간색)
        MeshFilter mf = effectGo.AddComponent<MeshFilter>();
        MeshRenderer mr = effectGo.AddComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = new Color(1f, 0f, 0f, 0.5f);
        mr.material = mat;

        // 원 모양 Mesh 생성 (세그먼트 30개 사용)
        Mesh mesh = new Mesh();
        int segments = 30;
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        // 중심점은 (0,0)
        vertices[0] = Vector3.zero;
        float angleInc = (2 * Mathf.PI) / segments;
        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = i * angleInc;
            vertices[i] = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0) * attackCircleSize;
        }

        // 삼각형 배열 생성 (중심과 인접 외곽 점들을 연결)
        for (int i = 0; i < segments; i++)
        {
            int currentVertex = i + 1;
            int nextVertex = (i + 1) % segments + 1;
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = currentVertex;
            triangles[i * 3 + 2] = nextVertex;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mf.mesh = mesh;

        // 효과 영역(attackCircleSize 반지름) 내의 모든 몬스터에게 데미지 적용
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

        // showTime 초 후에 효과 GameObject 삭제
        Destroy(effectGo, showTime);
    }
}