using UnityEngine;

/// <summary>
/// 플레이어의 레벨, 경험치 획득, 레벨업 로직을 관리하는 클래스
/// </summary>
public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExp = 0;

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;


    // 각 레벨업에 필요한 경험치 테이블 (1~50레벨 기준)
    private readonly int[] expRequirements = new int[]
    {
        0,    // 1레벨
        100,  // 2레벨
        150,  // 3레벨
        200,  // 4레벨
        250,  // 5레벨
        300,  // 6레벨
        350,  // 7레벨
        400,  // 8레벨
        450,  // 9레벨
        500,  // 10레벨
        600,  // 11레벨
        700,  // 12레벨
        800,  // 13레벨
        900,  // 14레벨
        1000, // 15레벨
        1150, // 16레벨
        1300, // 17레벨
        1450, // 18레벨
        1600, // 19레벨
        1800, // 20레벨
        2000, // 21레벨
        2200, // 22레벨
        2400, // 23레벨
        1600, // 24레벨
        2800, // 25레벨
        3000, // 26레벨
        3300, // 27레벨
        3600, // 28레벨
        3900, // 29레벨
        4200, // 30레벨
        4500, // 31레벨
        5000, // 32레벨
        5500, // 33레벨
        6000, // 34레벨
        6500, // 35레벨
        7000, // 36레벨
        7500, // 37레벨
        8000, // 38레벨
        8500, // 39레벨
        9000, // 40레벨
        9500, // 41레벨
        10000,// 42레벨
        11000,// 43레벨
        12000,// 44레벨
        13000,// 45레벨
        14000,// 46레벨
        15000,// 47레벨
        16000,// 48레벨
        18000,// 49레벨
        20000 // 50레벨
    };

    private const int maxLevel = 50;  // 최대 레벨

    /// <summary>
    /// 경험치를 추가하고 레벨업이 필요한지 확인한다.
    /// </summary>
    /// <param name="amount">추가할 경험치</param>
    public void AddExperience(int amount)
    {
        Debug.Log($"경험치 획득! +{amount}");

        if (currentLevel >= maxLevel)
            return; // 최대 레벨 도달 시 무시

        currentExp += amount;

        // 경험치가 다음 레벨업 필요치 이상이면 레벨업 처리 반복
        while (currentLevel < maxLevel && currentExp >= GetExpToNextLevel())
        {
            currentExp -= GetExpToNextLevel();
            LevelUp();
        }
    }

    /// <summary>
    /// 다음 레벨업까지 필요한 경험치 반환
    /// </summary>
    /// <returns></returns>
    private int GetExpToNextLevel()
    {
        if (currentLevel < expRequirements.Length)
            return expRequirements[currentLevel];
        else
            return int.MaxValue;  // 최대 레벨 이후에는 경험치 필요 없음
    }

    /// <summary>
    /// 레벨업 처리 (레벨 증가, 로그 출력)
    /// </summary>
    private void LevelUp()
    {
        currentLevel++;
        Debug.Log($"레벨업! 현재 레벨: {currentLevel}");
        // 레벨업 시 UI 업데이트, 효과음 재생 등 추가 가능
    }
}
