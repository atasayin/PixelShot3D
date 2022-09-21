using UnityEngine;

public class FallInWhiteColor : IBallEffect
{
    private Rigidbody _rb;
    private MeshRenderer _mr;

    public FallInWhiteColor(Rigidbody rb, MeshRenderer mr)
    {
        _rb = rb;
        _mr = mr;
    }

    public void CollisionEffect(Collider collider)
    {
        //Debug.Log("FallInWhiteColor");
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _mr.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", Color.white);
        _mr.SetPropertyBlock(mpb);
        _rb.useGravity = true;
        _rb.AddForce(new Vector3(0, 0, -float.Epsilon));
    }
}

