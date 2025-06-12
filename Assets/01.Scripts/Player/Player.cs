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
    public float AttackSpeed => attackSpeed;

    [Header("전투 스탯")]
    [SerializeField] protected float attack;           // 공격력
    [SerializeField] protected float specialAttack;    // 특수 공격력
    [SerializeField] protected float skillAttack;      // 특수 공격력
    [SerializeField] protected float defense;          // 방어력
    [SerializeField] protected float critical;         // 크리티컬 확률 (%)

    public float Attack => attack;
    public float Critical => critical;
    
    [Header("회복 스탯")]
    [SerializeField] public float hpRecovery;
    [SerializeField] public float mpRecovery;


    void Start()
    {
        GameManager.Instance.player = this;
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        if (Rb == null)
            Debug.LogError("Rigidbody not found on Player!");

        col = GetComponent<CapsuleCollider>();
    }
}