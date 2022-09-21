using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/IntAsset")]
public class IntAsset: ScriptableObject
{
    public int _value;

    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }
}
