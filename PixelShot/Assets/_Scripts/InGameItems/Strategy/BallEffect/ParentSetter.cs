using System;
using UnityEngine;

public class ParentSetter : IBallEffect
{
    private Transform _parent;
    private Transform _itself;

    public ParentSetter(Transform itself, Transform parent)
    {
        _parent = parent;
        _itself = itself;
    }

    public void CollisionEffect(Collider collider)
    {
        _itself.SetParent(_parent);
    }
}

