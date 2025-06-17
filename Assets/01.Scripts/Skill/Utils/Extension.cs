using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    // 원래는 isBool을 체크해줬는데, 그것보단 직접 하이어아키창에 있는지 없는지를 체크 할 용도
    public static bool IsValid(this SkillBase sb)
    {
        return sb != null && sb.isActiveAndEnabled;
    }
    public static bool IsValid(this MonsterBase mb)
    {
        return mb != null && mb.isActiveAndEnabled;
    }

    // List를 섞는 용도. 랜덤한놈 뽑아서 리스트 끝으로 보내고, 그놈 제외, 반복
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
