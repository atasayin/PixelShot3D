using UnityEngine;
using System.Collections.Generic;

public class Diamond : LevelItemMonoBase
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private AudioStruct deathAudio;

    protected override void Awake()
    {
        base.Awake();
        originalColor = Color.red;

        poolables = new List<IPoolable>();

        poolables.Add(new Pool_NonUseRigidbody(rigidbody));
        poolables.Add(new Pool_NonUseTransform(transform));
    }


    protected override void SetUpDeathEffectBehaviours()
    {
        base.SetUpDeathEffectBehaviours();
        deathEffects.Add(new GenerateDeathScorePopUp(transform, originalColor));
        deathEffects.Add(new ScoreDiamond());
        deathEffects.Add(new DeathSound(deathAudio, transform.position));
    }


    
}
