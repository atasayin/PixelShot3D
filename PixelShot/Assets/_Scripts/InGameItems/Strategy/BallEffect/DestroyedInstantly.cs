using System;
using UnityEngine;
public class DestroyedInstantly : IBallEffect
{
    private GameObject _objectToDestroy;

    public DestroyedInstantly(GameObject objectToDestroy)
    {
        _objectToDestroy = objectToDestroy;
    }

    public void CollisionEffect(Collider collider)
    {
        _objectToDestroy.SetActive(false);
    }
}
