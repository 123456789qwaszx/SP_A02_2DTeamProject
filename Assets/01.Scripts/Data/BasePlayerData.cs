[System.Serializable]
public class BasePlayerData
{
    // 모든 플레이어가 공통적으로 가지는 기본 스탯
    public float moveSpeed;
    public float attackSpeed;
    public float str;
    public float dex;
    public float ints;
    public float hp;
    public float mp;
    public float maxHP;
    public float maxMP;
    public float hpRegenAmount;
    public float mpRegenAmount;
    public float hpRegenInterval;
    public float mpRegenInterval;
    public float itemAP;

    public float projectileSpeed;
    public int projectileCount;
    public float attackRangeMultiplier;

    // 전투 스탯
    public float attack;
    public float specialAttack;
    public float skillAttack;
    public float defense;
    public float critical;
    public float critMultiplier;

    // 회복 스탯
    public float hpRecovery;
    public float mpRecovery;

    public void CopyFrom(BasePlayerData source)
    {
        this.moveSpeed = source.moveSpeed;
        this.attackSpeed = source.attackSpeed;
        this.str = source.str;
        this.dex = source.dex;
        this.ints = source.ints;
        this.hp = source.hp;
        this.mp = source.mp;
        this.maxHP = source.maxHP;
        this.maxMP = source.maxMP;
        this.hpRegenAmount = source.hpRegenAmount;
        this.mpRegenAmount = source.mpRegenAmount;
        this.hpRegenInterval = source.hpRegenInterval;
        this.mpRegenInterval = source.mpRegenInterval;
        this.itemAP = source.itemAP;
        this.projectileSpeed = source.projectileSpeed;
        this.projectileCount = source.projectileCount;
        this.attackRangeMultiplier = source.attackRangeMultiplier;
        this.attack = source.attack;
        this.specialAttack = source.specialAttack;
        this.skillAttack = source.skillAttack;
        this.defense = source.defense;
        this.critical = source.critical;
        this.critMultiplier = source.critMultiplier;
        this.hpRecovery = source.hpRecovery;
        this.mpRecovery = source.mpRecovery;
    }

}
