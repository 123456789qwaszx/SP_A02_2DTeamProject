using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{

    public HashSet<MonsterBase> Monsters { get; } = new HashSet<MonsterBase>();

    public List<MonsterBase> GetMonsters(int count = 1)
    {
        List<MonsterBase> monsterList = Monsters.ToList();

        int min = Mathf.Min(count, monsterList.Count);

        List<MonsterBase> randomMonsters = monsterList.Take(min).ToList();

        if (randomMonsters.Count == 0) return null;

        // ��� ������ count�� �ٸ� ��� ������ ��� �ݺ��ؼ� �߰�
        while (randomMonsters.Count < count)
        {
            randomMonsters.Add(randomMonsters.Last());
        }

        randomMonsters.Shuffle();
        return randomMonsters.Take(count).ToList();
    }

    public List<MonsterBase> GetMonsterWithinCamera(int count = 1)
    {
        List<MonsterBase> monsterList = Monsters.ToList().Where(monster => IsWithInCamera(Camera.main.WorldToViewportPoint(monster.transform.position)) == true).ToList();
        monsterList.Shuffle();

        int min = Mathf.Min(count, monsterList.Count);

        List<MonsterBase> monsters = monsterList.Take(min).ToList();

        if (monsters.Count == 0) return null;

        while (monsters.Count < count)
        {
            monsters.Add(monsters.Last());
        }

        return monsterList.Take(count).ToList();
    }

    
    public List<Transform> GetFindMonstersInFanShape(Vector3 origin, Vector3 forward, float radius = 2, float angleRange = 80)
    {
        List<Transform> listMonster = new List<Transform>();
        LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
        RaycastHit2D[] _targets = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 0, targetLayer);

        foreach (RaycastHit2D target in _targets)
        {
            float dot = Vector3.Dot((target.transform.position - origin).normalized, forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;
            // �þ߰� �Ǻ�
            if (degree <= angleRange / 2f)
                listMonster.Add(target.transform);
        }

        return listMonster;
    }
    
    
    bool IsWithInCamera(Vector3 pos)
    {
        if(pos.x >= 0 && pos.x <=1 && pos.y >= 0 && pos.y <= 1)
            return true;
        return false;
    }
}
