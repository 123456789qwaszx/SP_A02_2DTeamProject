using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour
{
    public bool isvalid;

    void Start()
    {
        
    }

    public void OnDamaged(Player attacker, int damage)
    {
        Debug.Log("몬스터대미지");
    }
}
