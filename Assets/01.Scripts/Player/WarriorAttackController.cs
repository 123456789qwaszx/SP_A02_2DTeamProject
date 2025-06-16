using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Warrior))]
public class WarriorAttackController : MonoBehaviour
{
    [Header("공격 범위 설정")]
    public float attackRange = 3f;                // 부채꼴 반지름
    [Range(0, 180)]
    public float attackAngle = 60f;               // 부채꼴 각도

    [Header("몬스터 레이어")]
    public LayerMask monsterLayer;                // 공격 대상 레이어

    [Header("시각화 설정")]
    public float showTime = 0.3f;                 // 공격 효과 유지 시간 (초)
    public float attackCircleRadius = 0.8f;       // 공격 효과가 생성될 가상의 원 반지름 (플레이어 중심에서의 오프셋)

    private Warrior warrior;

    void Awake()
    {
        warrior = GetComponent<Warrior>();
    }

    /// <summary>
    /// 마우스 클릭을 받아 공격을 실행
    /// 공격 판정은 한 번만 실행되며, 워리어 스크립트에 구현된 GetDamage() 함수를 호출
    /// Attack 또는 critDamage_Normal 중 하나의 데미지 값을 가져옴
    /// 공격 효과는 플레이어의 움직임을 따라, 가상의 원 위에서 생성
    /// </summary>
    /// <param name="mouseWorldPos">Camera.ScreenToWorldPoint()로 얻은 월드 좌표 (z=0)</param>
    public void AttackByMouse(Vector3 mouseWorldPos)
    {
        // 공격 시점의 플레이어 위치
        Vector3 attackOrigin = transform.position;
        // 마우스 클릭으로부터 기본 방향 계산
        Vector3 computedDir = (mouseWorldPos - attackOrigin).normalized;

        // SpriteRenderer를 통해 플레이어의 좌우 방향 결정  
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Vector3 playerFacing = (sr != null) ? (sr.flipX ? Vector3.right : Vector3.left) : Vector3.right;

        // 플레이어가 보는 방향에 맞게 computedDir의 x값 보정
        computedDir.x = Mathf.Sign(playerFacing.x) * Mathf.Abs(computedDir.x);
        computedDir = computedDir.normalized;

        // 공격 판정: 부채꼴 영역 내의 몬스터에 대해 한 번만 판정
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, attackRange, monsterLayer);
        float halfAngle = attackAngle * 0.5f;

        float finalDamage = warrior.GetDamage();

        foreach (Collider2D col in hits)
        {
            Vector3 toTarget = (col.transform.position - attackOrigin).normalized;
            if (Vector3.Angle(computedDir, toTarget) <= halfAngle)
            {
                if (col != null)
                {
                    IDamagable damagable = col.GetComponent<IDamagable>();
                    if (damagable != null)
                    {
                        damagable.TakeDamage(finalDamage);
                    }
                }
            }
        }

        CreateStaticAttackEffect(attackOrigin, computedDir);
    }

    /// <summary>
    /// 공격 시점의 위치와 방향을 기준으로, 가상의 원 위에서 부채꼴 Mesh 효과 GameObject를 생성
    /// showTime 동안 플레이어의 현재 위치를 따라, 고정된 오프셋(attackCircleRadius와 direction)을 유지하며, 일정 시간 후 자동으로 삭제
    /// </summary>
    /// <param name="origin">공격 시작 시점의 플레이어 위치</param>
    /// <param name="direction">마우스 클릭 방향 (정규화된 벡터, 플레이어 좌우 방향 반영됨)</param>
    private void CreateStaticAttackEffect(Vector3 origin, Vector3 direction)
    {
        // 가상의 원 위에서 효과의 중심 위치 계산
        Vector3 effectCenter = origin + direction * attackCircleRadius;

        // 새 GameObject 생성 (부모에 붙이지 않고, 직접 이동 업데이트)
        GameObject effectGo = new GameObject("AttackEffect");
        effectGo.transform.position = effectCenter;
        effectGo.transform.rotation = Quaternion.identity;
        effectGo.tag = "AttackEffect";

        // Mesh 컴포넌트 추가
        MeshFilter mf = effectGo.AddComponent<MeshFilter>();
        MeshRenderer mr = effectGo.AddComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Sprites/Default"));
        mr.material.color = new Color(1f, 0f, 0f, 0.5f);

        // Mesh 생성
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        // 로컬 좌표계 기준: 중앙(0,0,0)에서 부채꼴 Mesh 생성
        float centralAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int segments = 20;
        float halfAng = attackAngle * 0.5f;
        List<Vector3> vertices = new List<Vector3> { Vector3.zero };

        // 부채꼴 가장자리 점들 계산
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = centralAngle - halfAng + ((attackAngle / segments) * i);
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * attackRange;
            vertices.Add(point);
        }

        // 삼각형 인덱스 생성
        List<int> triangles = new List<int>();
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        // 공격 효과 오브젝트가 플레이어의 현재 위치를 따라가도록 코루틴 실행
        StartCoroutine(FollowEffect(effectGo, direction, showTime));
    }

    /// <summary>
    /// 지정된 시간 동안 매 프레임 플레이어의 현재 좌표에 대해, 
    /// 효과 오브젝트가 플레이어 중심에서 direction * attackCircleRadius 만큼 떨어진 위치로 업데이트
    /// </summary>
    /// <param name="effectGo">공격 효과 GameObject</param>
    /// <param name="direction">공격 방향 (정규화된 벡터)</param>
    /// <param name="duration">효과 유지 시간</param>
    /// <returns></returns>
    private IEnumerator FollowEffect(GameObject effectGo, Vector3 direction, float duration)
    {
        float elapsed = 0f;
        // 매 프레임 플레이어의 위치에 대해 고정된 오프셋 적용
        Vector3 offset = direction * attackCircleRadius;
        while (elapsed < duration)
        {
            effectGo.transform.position = transform.position + offset;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(effectGo);
    }
}