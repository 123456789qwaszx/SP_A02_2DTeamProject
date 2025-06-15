using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterSpawner : MonoBehaviour
{
    
    public GameObject[] eliteMonsterPrefabs;

    void Start()
    {

    }

    void Update()
    {
        SpawnFromPool2(eliteMonsterPrefabs);
        Debug.Log(ObjectManager.Instance.Monsters.Count);
    }


    void SpawnFromPool2(GameObject[] prefabs)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject monster = PoolManager.Instance.Pop(prefab);
        MonsterBase monsterBase = monster.GetComponent<MonsterBase>();
        ObjectManager.Instance.Monsters.Add(monsterBase);
    }

    void InitPooledMonster2(GameObject monster, Vector3 pos) // 스폰 몬스터 필드에 셋팅
    {
        monster.transform.position = pos;
        monster.transform.rotation = Quaternion.identity;   // 반전처리 해야함
        monster.SetActive(true);

        IMonster resettable = monster.GetComponent<IMonster>();
        if (resettable != null)
            resettable.ResetMonster();
    }
}
