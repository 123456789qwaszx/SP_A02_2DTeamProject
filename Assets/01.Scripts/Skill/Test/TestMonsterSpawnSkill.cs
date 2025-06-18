using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterSpawnSkill : MonoBehaviour
{
    public GameObject bear_Prefab;
    public GameObject SpawnBear() { return PoolManager.Instance.Pop(bear_Prefab); }
    public void DespawnBear(GameObject bear) { PoolManager.Instance.Push(bear); }

    void Start()
    {
        StartCoroutine(CoTestSpawn());

    }

    IEnumerator CoTestSpawn()
    {
        while (true)
        {
            GameObject go = SpawnBear();
            MonsterBase monsterBase = go.GetComponent<MonsterBase>();
            ObjectManager.Instance.Monsters.Add(monsterBase);
        
        yield return new WaitForSeconds(1f);
        }
    }
}
