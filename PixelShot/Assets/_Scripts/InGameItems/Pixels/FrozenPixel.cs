using System.Collections.Generic;
using UnityEngine;

public class FrozenPixel : PixelBehaviourBase
{

    protected override void Awake()
    {
        base.Awake();
        poolables = new List<IPoolable>();
        ballEffects = new List<IBallEffect>();
        originalColor = Color.blue;
        mr = GetComponent<MeshRenderer>();
        originalColor = mr.sharedMaterial.color;

        poolables.Add(new Pool_NonUseTransform(transform));
        poolables.Add(new Pool_ChangeColor(mr, originalColor));

        ballEffects.Add(new DestroyedInstantly(gameObject));
        ballEffects.Add(new SlowColliedOject());
    }


    private void OnTriggerEnter(Collider other)
    {
        if(LevelManager.Instance.IsPlaying)
        {
            if (!_isCollided)
            {
                _isCollided = true;
                RunBallEffectBehaviours(other);
            }
        }
    }

   

}
