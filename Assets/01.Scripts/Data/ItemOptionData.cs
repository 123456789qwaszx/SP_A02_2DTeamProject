using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemOptionData
{
    public string optionName;
    public float value;

    public ItemOptionData() { }

    public ItemOptionData(string name, float val)
    {
        optionName = name;
        value = val;
    }
}
