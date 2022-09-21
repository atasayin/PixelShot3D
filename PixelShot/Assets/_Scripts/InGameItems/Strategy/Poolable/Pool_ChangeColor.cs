using System;
using UnityEngine;

public class Pool_ChangeColor : IPoolable
{

    private MeshRenderer _mr;
    private Color32 _original;

    public Pool_ChangeColor(MeshRenderer mr, Color original)
    {
        _mr = mr;
        _original = original;
    }

    public void GetPooledObject()
    {
       MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _mr.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", _original);
        _mr.SetPropertyBlock(mpb);
    }

    public void ReturnToPool()
    {
       
    }


    
}

