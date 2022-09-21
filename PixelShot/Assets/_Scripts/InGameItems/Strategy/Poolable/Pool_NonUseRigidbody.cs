using UnityEngine;

public class Pool_NonUseRigidbody : IPoolable
{
    private Rigidbody _rb;

    public Pool_NonUseRigidbody(Rigidbody rb)
    {
        _rb = rb;
    }

    public void GetPooledObject()
    { 
        _rb.useGravity = false;
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void ReturnToPool()
    {
    }
}

