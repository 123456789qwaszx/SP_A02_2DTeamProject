using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName;

    [Header("스탯")]
    public int maxHP;
    public int attackPower;
    public float moveSpeed;
    public float attackCooldown;
    public float attackRange;

    [Header("분류")]
    public MonsterType type;

    public enum MonsterType
    {
        Melee_Normal,
        Melee_Tanker,
        Melee_Dasher,
        Melee_Elite,
        Ranged_Normal,
        Ranged_Elite,
        MidBoss,
        Boss,
        FinalBoss
    }
}
