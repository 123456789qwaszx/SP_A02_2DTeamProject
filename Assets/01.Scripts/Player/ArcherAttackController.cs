using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Archer))]
public class ArcherAttackController : MonoBehaviour
{
    [Header("공격 범위 설정")]
    public float attackRange = 7f;
    public float arrowMoveSpeed = 3f;

    [Header("몬스터 레이어")]
    public LayerMask monsterLayer; // 공격 대상 레이어

    [SerializeField]
    private GameObject arrowPrefab; // Inspector에서 할당 (Assets/03.Prefabs/Player/Arrow.prefab)

    public Archer archer;

    void Awake()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("발사체(Arrow) 프리팹이 Inspector에 할당되지 않았습니다.");
        }
        archer = GetComponent<Archer>();
    }

    /// <summary>
    /// 마우스 클릭을 받아 공격을 실행합니다.
    /// </summary>
    /// <param name="mouseWorldPos">
    /// Camera.ScreenToWorldPoint()로 얻은 월드 좌표 (z=0)
    /// </param>
    public void ShootArrow(Vector3 mouseWorldPos)
    {
        // 공격 시점의 플레이어 위치
        Vector3 attackOrigin = transform.position;
        // 마우스 클릭으로부터 기본 방향(정규화된 벡터) 계산
        Vector3 shootDirection = (mouseWorldPos - attackOrigin).normalized;

        // 플레이어 좌우 방향 보정을 원한다면 아래 코드를 사용합니다.
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Vector3 playerFacing = (sr != null) ? (sr.flipX ? Vector3.right : Vector3.left) : Vector3.right;
        // shootDirection.x = Mathf.Sign(playerFacing.x) * Mathf.Abs(shootDirection.x);

        // 화살을 생성 및 초기화합니다.
        AttackEffect(attackOrigin, shootDirection);
    }

    /// <summary>
    /// Inspector에서 할당된 Arrow 프리팹을 사용하여 공격 효과를 생성
    /// </summary>
    /// <param name="origin">공격 시작 시점의 플레이어 위치</param>
    /// <param name="direction">마우스 클릭 방향 (정규화된 벡터)</param>
    private void AttackEffect(Vector3 origin, Vector3 direction)
    {
        GameObject arrow = Instantiate(arrowPrefab, origin, Quaternion.identity);
        Debug.Log("Arrow Instantiated at: " + origin);

        // 플레이어의 Collider와 화살의 Collider 충돌 무시
        Collider2D arrowCollider = arrow.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>(); 
        if (arrowCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(arrowCollider, playerCollider);
        }

        // 화살 이동 및 충돌 처리를 위한 스크립트를 초기화합니다.
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.arrowMoveSpeed = arrowMoveSpeed;
            arrowScript.attackRange = attackRange;
            arrowScript.monsterLayer = monsterLayer;
            arrowScript.Initialize(direction);
        }
    }
}