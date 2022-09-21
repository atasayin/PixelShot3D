using UnityEngine;

public class SlowColliedOject : IBallEffect
{

    private const float SLOW_COEF = 0.95f; 

    public void CollisionEffect(Collider collider)
    {
        collider.attachedRigidbody.velocity *= SLOW_COEF;
    }

   
}
