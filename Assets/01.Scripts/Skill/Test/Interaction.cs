using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Interaction : MonoBehaviour
{
    public Action<SkillBook> OnPlayerInteraction;
    public float InteractInterval = 0.5f;
    private SkillBook _player;

    private void Start()
    {
        StartCoroutine(CoPlayerInteraction());
    }

    IEnumerator CoPlayerInteraction()
    {
        while (true)
        {
            yield return new WaitForSeconds(InteractInterval);

            if (_player != null)
                OnPlayerInteraction(_player);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        SkillBook pc = other.GetComponent<SkillBook>();
        if (pc == null)
            return;

        _player = pc;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        SkillBook pc = other.GetComponent<SkillBook>();
        if (pc == null)
            return;

        _player = null;
    }
}
