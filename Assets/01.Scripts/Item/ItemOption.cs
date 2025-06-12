using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemOption
{
    public string optionName;
    public float minValue;
    public float maxValue;
    
    public float GetRandomValue() => Random.Range(minValue, maxValue);
}
