using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ArcherAttackController
{
    private Vector3 initialPosition;       // 발사 시 초기 위치
    private Vector2 direction;             // 화살 진행 방향
    private float damage;                  // 화살 데미지 값 (부모 아처의 GetDamage() 값 사용)

    // 초기 설정: 화살의 진행 방향을 설정하고 부모 아처의 데미지 값을 가져 옴
    public void Initialize(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;  // 정규화된 방향 설정
        initialPosition = transform.position;

        // 부모 아처 컴포넌트를 통해 데미지 값을 가져옴
        Archer archer = GetComponentInParent<Archer>();
        if (archer != null)
        {
            damage = archer.GetDamage();
        }

        // 화살이 진행 방향에 맞게 회전 (local right 방향이 진행 방향이라고 가정)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        // 설정된 방향으로 화살 이동
        transform.position += (Vector3)direction * arrowMoveSpeed * Time.deltaTime;

        // 초기 위치와의 거리가 attackRange 이상이면 화살 삭제
        if (Vector3.Distance(initialPosition, transform.position) >= attackRange)
        {
            Destroy(gameObject);
        }
    }

    // 화살이 충돌하면, 몬스터와 충돌한 경우에는 데미지를 적용하고, 그 외엔 그냥 삭제
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Arrow collider: " + col.gameObject.name);
        if (((1 << col.gameObject.layer) & monsterLayer.value) != 0) // 몬스터 레이어 검사
        {
            IDamagable target = col.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        Destroy(gameObject); // 충돌하면 무조건 삭제
    }
}