using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    // 몬스터를 Spawn할 때 이 리스트에 넣어주십시오.
    // 지금은 프리팹 세팅 시, GameObject로 해두셔서
    // 아래처럼 변환해 주신 다음, 넣어주시면 될 것 같습니다.
    // MonsterBase monsterBase = monster.GetComponent<MonsterBase>();
    // ObjectManager.Instance.Monsters.Add(monsterBase);
    public HashSet<MonsterBase> Monsters { get; } = new HashSet<MonsterBase>();

    public List<MonsterBase> GetMonsterWithinCamera(int count = 1)
    {
        List<MonsterBase> monsterList = Monsters.ToList().Where(monster => IsWithInCamera(Camera.main.WorldToViewportPoint(monster.transform.position)) == true).ToList();
        monsterList.Shuffle();

        int min = Mathf.Min(count, monsterList.Count);

        //Linq.Take는 리스트 시작부터 ~ min까지를 뱉음
        List<MonsterBase> monsters = monsterList.Take(min).ToList();

        // 즉 count 0 으로 요청을 가라로 했거나, 화면상 몬스터가 0이라면 null반환
        if (monsters.Count == 0) return null;

        // 화면상 몬스터 숫자가, 요청보다 적다면 가장 뒤에 리스트를 배치해서 부족한 count만큼 가라로 채운다.(이러면 특정 한놈만 타겟팅될 확률이 늘겠지만, 어차피 랜덤으로 계속 셔플해주니 상관없어. 더 신경쓰는게 바보)
        while (monsters.Count < count)
        {
            monsters.Add(monsters.Last());
        }
        // 처음 count개의 항목만 반복해서 진행하면서 몬스터를 추가한다.
        return monsterList.Take(count).ToList();
    }

    
    public List<Transform> GetFindMonstersInFanShape(Vector3 origin, Vector3 forward, float radius = 2, float angleRange = 80)
    {
        List<Transform> listMonster = new List<Transform>();
        LayerMask targetLayer = LayerMask.GetMask("Monster");
        RaycastHit2D[] _targets = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 0, targetLayer);

        foreach (RaycastHit2D target in _targets)
        {
            float dot = Vector3.Dot((target.transform.position - origin).normalized, forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;
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
