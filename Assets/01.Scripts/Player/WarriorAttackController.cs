using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 전사 캐릭터의 공격을 처리
/// 마우스 클릭 좌표를 받아 공격 방향을 계산하고,  
/// 만약 클릭 위치가 플레이어 전면과 90° 이상 차이가 나면 y축 반전을 적용하여 공격 방향을 보정
/// 또한, 공격의 부채꼴 영역(피자 모양)을 Mesh로 채워진 면 형태로 표시하며, 효과가 지속되는 동안  
/// 플레이어의 움직임에 따라 공격 효과 Mesh가 따라오도록 함
/// </summary>
[RequireComponent(typeof(Warrior))]
public class WarriorAttackController : MonoBehaviour
{
    [Header("공격 범위 설정")]
    public float attackRange = 3f;                // 공격 사거리 (부채꼴 반지름)
    [Range(0, 180)] public float attackAngle = 60f; // 부채꼴 각도

    [Header("몬스터 레이어")]
    public LayerMask monsterLayer;                // 공격 대상 레이어

    [Header("시각화 설정")]
    public float showTime = 0.3f;                 // 공격 부채꼴 표시 지속 시간
    [Tooltip("공격 효과 Mesh가 생성될 때, 공격 기준점(데미지 판정 기준)에서 공격 방향으로 이동할 거리")]
    public float attackOffsetDistance = 0.8f;     // 부채꼴 Mesh 오프셋 (플레이어 앞으로)

    private Warrior warrior;
    private Transform playerTf;

    private Vector3 forwardDir;                   // 플레이어가 실제로 바라보는 전면 방향 

    private Mesh attackMesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        warrior = GetComponent<Warrior>();
        playerTf = transform;

        // AttackView라는 자식 오브젝트에서 MeshFilter와 MeshRenderer 찾기
        Transform attackViewTf = transform.Find("AttackView");
        if (attackViewTf == null)
        {
            Debug.LogError("[WarriorAttackController] AttackView 자식 오브젝트가 없음");
            enabled = false;
            return;
        }

        meshFilter = attackViewTf.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = attackViewTf.gameObject.AddComponent<MeshFilter>();
        }
        meshRenderer = attackViewTf.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = attackViewTf.gameObject.AddComponent<MeshRenderer>();
            // 간단한 Sprite Default Shader와 반투명 빨강 색상으로 재질 설정
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
            meshRenderer.material.color = new Color(1f, 0f, 0f, 0.5f);
        }
    }

    /// <summary>
    /// 마우스 좌클릭 시 호출
    /// mouseWorldPos는 Camera.ScreenToWorldPoint()로 얻은 값이며, z값은 0
    /// </summary>
    /// <param name="mouseWorldPos">마우스 클릭 월드 좌표 (z=0)</param>
    public void AttackByMouse(Vector3 mouseWorldPos)
    {
        // 공격 판정의 기준점은 공격 시작 시의 플레이어 위치
        Vector3 attackOrigin = playerTf.position;

        // SpriteRenderer를 통해 플레이어 시각적 방향(전면)을 결정
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            // Sprite 설정에 따라 flipX 값이 실제 바라보는 방향과 반대로 동작한다면 할당을 반전
            forwardDir = sr.flipX ? Vector3.right : Vector3.left;
            Debug.Log($"Sprite flipX: {sr.flipX}, forwardDir set to: {forwardDir}");
        }
        else
        {
            forwardDir = Vector3.right;
        }

        // 공격 판정은 공격 시작 시의 위치와 마우스 클릭 좌표를 기준으로 계산
        Vector3 computedDir = (mouseWorldPos - attackOrigin).normalized;

        // 디버그: 공격 시작 위치에서 forwardDir (녹색)와 computedDir (파란색) 표시
        Debug.DrawRay(attackOrigin, forwardDir * attackRange, Color.green, 2f);
        Debug.DrawRay(attackOrigin, computedDir * attackRange, Color.blue, 2f);

        // forwardDir과 computedDir 사이의 각도를 계산
        float angleDiff = Vector3.Angle(forwardDir, computedDir);
        Debug.Log($"Angle between forwardDir and computedDir: {angleDiff}°");

        // 만약 클릭 위치가 플레이어 뒤쪽(180도에 가까움)이라면, y축 기준(즉 x축 반전)을 적용하여 공격 방향을 보정
        if (angleDiff > 90f)
        {
            computedDir = new Vector3(-computedDir.x, computedDir.y, computedDir.z);
            Debug.Log("마우스 클릭 위치가 플레이어 뒤쪽이어서 y축 반전을 적용합니다.");
        }

        // 데미지 판정은 attackOrigin 기준
        DoSectorAttack(attackOrigin, computedDir);

        // 효과 Mesh는 플레이어의 움직임을 반영하도록 매 프레임 갱신
        StopAllCoroutines();
        StartCoroutine(ShowSectorFilledFollow(attackOrigin, computedDir, showTime));
    }

    /// <summary>
    /// 플레이어 주변의 모든 몬스터 중, attackOrigin을 기준으로 지정된 부채꼴 영역 내에 있는 몬스터에게 데미지 적용
    /// </summary>
    /// <param name="origin">공격 시작 시점의 플레이어 위치 (데미지 판정 기준)</param>
    /// <param name="direction">공격 방향 (정규화된 벡터)</param>
    private void DoSectorAttack(Vector3 origin, Vector3 direction)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, monsterLayer);
        float halfAng = attackAngle * 0.5f;
        foreach (var col in hits)
        {
            Vector3 toTarget = (col.transform.position - origin).normalized;
            float ang = Vector3.Angle(direction, toTarget);
            if (ang <= halfAng)
            {
                bool isCrit = Random.value < warrior.Critical;
                float dmg = isCrit ? warrior.critDamage_Normal : warrior.Attack;
                //col.GetComponent<Monster>()?.TakeDamage(dmg);
            }
        }
    }

    /// <summary>
    /// 채워진 부채꼴(피자 모양) Mesh 이펙트를 생성하여 표시하며, 효과 지속 시간 동안 플레이어의 움직임을 반영
    /// </summary>
    /// <param name="initialOrigin">공격 시작 시점의 플레이어 위치 (데미지 판정 기준, 고정)</param>
    /// <param name="direction">공격 방향 (정규화된 벡터, 고정)</param>
    /// <param name="duration">표시 지속 시간 (초)</param>
    private IEnumerator ShowSectorFilledFollow(Vector3 initialOrigin, Vector3 direction, float duration)
    {
        int segments = 30;
        float halfAng = attackAngle * 0.5f;
        float step = attackAngle / segments;
        float baseDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            Vector3 localOffset = (forwardDir.x < 0 ? Vector3.left : Vector3.right) * attackOffsetDistance;
            Vector3 effectCenter = playerTf.TransformPoint(localOffset);

            // 추가: 플레이어 위치와 effectCenter의 거리를 디버그 로그로 출력
            float dist = Vector3.Distance(playerTf.position, effectCenter);
            Debug.Log($"Player Position: {playerTf.position} | Effect Center: {effectCenter} | Distance: {dist}");

            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(effectCenter);

            for (int i = 0; i <= segments; i++)
            {
                float deg = baseDeg - halfAng + step * i;
                float rad = deg * Mathf.Deg2Rad;
                Vector3 vertex = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * attackRange + effectCenter;
                vertices.Add(vertex);
            }

            List<int> triangles = new List<int>();
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }

            if (attackMesh == null)
                attackMesh = new Mesh();
            else
                attackMesh.Clear();
            attackMesh.SetVertices(vertices);
            attackMesh.SetTriangles(triangles, 0);
            meshFilter.mesh = attackMesh;

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (attackMesh != null)
            attackMesh.Clear();
    }
}