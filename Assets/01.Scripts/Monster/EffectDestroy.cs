using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public float delay = 3f;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
