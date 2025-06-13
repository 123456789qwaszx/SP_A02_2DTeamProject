using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;

public interface IEquipable
{
    void ApplyOption(ItemOptionType type, float value);
    void RemoveOption(ItemOptionType type, float value);
}

public class Player : MonoBehaviour, IEquipable
{
    protected Rigidbody2D rb;
    protected Transform tf;

    public Rigidbody2D Rb => rb;
    public Transform Tf => tf;
    private CapsuleCollider col;


    
    public float GetHP() => hp;
    public float GetMP() => mp;
    public float GetMaxHP() => maxHP; // 임시값, 나중에 성장 시스템 연동 가능
    public float GetMaxMP() => maxMP;

    [Header("플레이어 스탯")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float STR;               // 힘
    [SerializeField] protected float DEX;               // 민첩
    [SerializeField] protected float INT;               // 지능
    [SerializeField] protected float hp;
    [SerializeField] protected float mp;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float maxMP;
    [SerializeField] protected float hpRegenAmount;  // HP회복량 증가
    [SerializeField] protected float mpRegenAmount;  // MP회복량 증가
    [SerializeField] protected float hpRegenInterval; // HP회복속도 증가
    [SerializeField] protected float mpRegenInterval; // MP회복속도 증가
    [SerializeField] protected float itemAP;            // Item Acquisition Probability 아이템 획득 확률
    
    [SerializeField] protected float projectileSpeed = 1.0f;  // 투사체 속도 배율
    [SerializeField] protected int projectileCount = 1;       // 투사체 수
    [SerializeField] protected float attackRangeMultiplier = 1.0f; // 공격 범위 배율

    public float MoveSpeed => moveSpeed;
    public float AttackSpeed => attackSpeed;

    [Header("전투 스탯")]
    [SerializeField] protected float attack;           // 공격력
    [SerializeField] protected float specialAttack;    // 특수 공격력
    [SerializeField] protected float skillAttack;      // 특수 공격력
    [SerializeField] protected float defense;          // 방어력
    [SerializeField] protected float critical;         // 크리티컬 확률 (%)
    [SerializeField] protected float critMultiplier;   // 치명타 피해량 증가

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

    public void ApplyOption(ItemOptionType type, float value)
    {
        switch (type)
        {
            case ItemOptionType.공격력: attack += value; break;
            case ItemOptionType.방어력: defense += value; break;
            case ItemOptionType.치명타확률: critical += value; break;
            case ItemOptionType.치명타피해량: critMultiplier += value; break;
            case ItemOptionType.힘: STR += value; break;
            case ItemOptionType.민첩: DEX += value; break;
            case ItemOptionType.지능: INT += value; break;
            case ItemOptionType.공격속도: attackSpeed += value; break;
            case ItemOptionType.이동속도: moveSpeed += value; break;
            case ItemOptionType.행운: itemAP += value; break;
            case ItemOptionType.특수공격력증가: specialAttack += value; break;
            case ItemOptionType.최대HP량증가: maxHP += value; break;
            case ItemOptionType.최대MP량증가: maxMP += value; break;
            case ItemOptionType.초당HP회복량증가: hpRegenAmount += value; break;
            case ItemOptionType.초당MP회복량증가: mpRegenAmount += value; break;
            case ItemOptionType.초당HP회복속도증가: hpRegenInterval *= (1f - value); break;
            case ItemOptionType.초당MP회복속도증가: mpRegenInterval *= (1f - value); break;
            case ItemOptionType.투사체수증가: projectileCount += (int)value; break;
            case ItemOptionType.투사체속도증가: projectileSpeed += value; break; // 배율식 증가
            case ItemOptionType.공격범위증가: attackRangeMultiplier += value; break; // 예: 1.2배
            // 필요한 항목만 점차 추가해도 됨
        }
    }

    public void RemoveOption(ItemOptionType type, float value)
    {
        ApplyOption(type, -value);
    }
}