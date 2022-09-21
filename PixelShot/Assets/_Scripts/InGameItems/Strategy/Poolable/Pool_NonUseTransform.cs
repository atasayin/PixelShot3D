using UnityEngine;

public class Pool_NonUseTransform : IPoolable
{

    private Transform _ts;
    private Vector3 _startPosition;

    public Pool_NonUseTransform(Transform ts)
    {
        _ts = ts;
    }

    public Pool_NonUseTransform(Transform ts, Vector3 startPosition)
    {
        _ts = ts;
        _startPosition = startPosition;

    }

    public void GetPooledObject()
    {
        if (_startPosition != Vector3.zero)
        {
            _ts.position = _startPosition;   
        }
    }
       

    public void ReturnToPool()
    {
        _ts.SetPositionAndRotation(new Vector3(1000, 1000, 1000), Quaternion.identity);
    }
}

