using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimplePixel : PixelBehaviourBase, IBallLineTriggerEffect
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AudioStruct deathAudio;

    protected override void Awake()
    {
        base.Awake();
        poolables = new List<IPoolable>();
        ballEffects = new List<IBallEffect>();
        
        poolables.Add(new Pool_NonUseRigidbody(_rb) );
        poolables.Add(new Pool_NonUseTransform(transform) );
      
        ballEffects.Add(new FallInWhiteColor(_rb, mr));
        ballEffects.Add(new ScorablePixelCountProgress());
        ballEffects.Add(new DeathInSeconds(gameObject, this));
        ballEffects.Add(new ParentSetter(transform, ObjectManager.Instance.transform));

    }

    public void OnBallLineEffet()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mr.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", originalColor);
        mr.SetPropertyBlock(mpb);
    }

    protected override void SetUpDeathEffectBehaviours()
    {
        base.SetUpDeathEffectBehaviours();
        deathEffects.Add(new GenerateDeathScorePopUp(transform));
        deathEffects.Add(new DeathVibration(this));
        deathEffects.Add(new DeathSound(deathAudio, transform.position));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (LevelManager.Instance.IsPlaying)
        {
            if (!_isCollided)
            {
                _isCollided = true;
                RunBallEffectBehaviours(collision.collider);
            }

        }

    }


    public Color32 OriginalColor
    {
        get { return originalColor; }
        set { originalColor = value;  }
    }









}
