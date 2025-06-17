[System.Serializable]
public class PlayerData
{
    // 기본 스탯은 BasePlayerData 타입
    public BasePlayerData baseData;

    // 직업별 확장 데이터 선언 
    public WarriorData warriorData;
    public WizardData wizardData;
    public ArcherData archerData;

    public string jobType;
}