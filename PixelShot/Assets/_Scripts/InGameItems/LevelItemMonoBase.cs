using System.Collections.Generic;
using UnityEngine;


public class LevelItemMonoBase : MonoBehaviour
{
    protected List<IPoolable> poolables;
    protected List<IDeathEffect> deathEffects;
    public bool _isCollided = false;
    protected Color32 originalColor;

    protected virtual void Awake()
    {
        deathEffects = new List<IDeathEffect>();
    }

    protected virtual void OnEnable()
    {
        _isCollided = false;
        RunPoolableGetBehaviours();
    }

    protected virtual void OnDisable()
    {
        RunPoolableReturnBehaviours();
    }

  

    protected virtual void SetUpDeathEffectBehaviours()
    {
        deathEffects.Add(new GenerateDeathParticles(transform, originalColor));
    }


    public void RunDeathEffectBehaviours()
    {
        SetUpDeathEffectBehaviours();

        foreach (IDeathEffect deathEffect in deathEffects)
        {
            deathEffect.InvokeDeathEffect();
        }
    }

    public void RunPoolableGetBehaviours()
    {
        foreach (IPoolable poolable in poolables)
        {
            poolable.GetPooledObject();
        }
    }

    public void RunPoolableReturnBehaviours()
    {
        foreach (IPoolable poolable in poolables)
        {
            poolable.ReturnToPool();
        }
    }






}
