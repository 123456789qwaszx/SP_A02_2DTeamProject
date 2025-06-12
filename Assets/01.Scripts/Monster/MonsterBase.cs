using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMonster
{
    void ResetMonster();
}
public class MonsterBase : MonoBehaviour, IMonster
{
    public int maxHP = 100;
    private int currentHP;

    public void ResetMonster()
    {
        currentHP = maxHP;
        // 기타 초기화 추가 예정
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            PoolManager.Instance.Push(this.gameObject);
        }
    }
}
