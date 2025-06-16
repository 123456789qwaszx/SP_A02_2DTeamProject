using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName;

    [Header("Status")]
    public int maxHP;
    public int attackPower;
    public float moveSpeed;
    public float attackCooldown;
    public float attackRange;

    [Header("몬스터타입 분류")]
    public MonsterType type;

    [Header("발사체")]
    public GameObject projectilePrefab;
    public GameObject bossProjectilePrefab;

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
