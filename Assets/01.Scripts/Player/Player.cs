using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Transform tf;

    public Rigidbody2D Rb => rb;
    public Transform Tf => tf;
    private CapsuleCollider col;


    [Header("플레이어 스탯")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float STR;               // 힘
    [SerializeField] protected float DEX;               // 민첩
    [SerializeField] protected float INT;               // 지능
    [SerializeField] protected float hp;
    [SerializeField] protected float mp;
    [SerializeField] protected float itemAP;            // Item Acquisition Probability 아이템 획득 확률

    public float MoveSpeed => moveSpeed;

    [Header("전투 스탯")]
    [SerializeField] protected float attack;       // 공격력
    [SerializeField] protected float defense;      // 방어력
    [SerializeField] protected float critical;    // 크리티컬 확률 (%)

    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider>();
    }
}