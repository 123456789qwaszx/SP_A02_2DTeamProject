using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Interaction interaction;
    private float workSpeed = 0.5f;


    void Start()
    {
        interaction.InteractInterval = workSpeed;
        interaction.OnPlayerInteraction = OnPlayerInteraction;
    }

    void OnPlayerInteraction(SkillBook pc)
    {
        ObjectManager.Instance.RemoveMonster();
        Debug.Log(ObjectManager.Instance.Monsters.Count);
    }
}