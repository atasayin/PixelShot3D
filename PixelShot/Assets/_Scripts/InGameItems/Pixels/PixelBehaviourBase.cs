using System.Collections.Generic;
using UnityEngine;

public class PixelBehaviourBase : LevelItemMonoBase
{

    [SerializeField] protected MeshRenderer mr;
    
    protected List<IBallEffect> ballEffects;

    protected void RunBallEffectBehaviours(Collider collider)
    {
        foreach (IBallEffect ballEffect in ballEffects)
        {
            ballEffect.CollisionEffect(collider);
        }

    }

}
