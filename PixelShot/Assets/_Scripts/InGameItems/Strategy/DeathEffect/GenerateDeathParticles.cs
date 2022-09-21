using UnityEngine;
using DG.Tweening;
public class GenerateDeathParticles : IDeathEffect
{
    private Transform _transform;
    private Color32 _originalColor;

    public GenerateDeathParticles(Transform transform, Color32 originalColor)
    {
        _transform = transform;
        _originalColor = originalColor;
    }


    public void InvokeDeathEffect()
    {
        GameObject particleObject = ObjectManager.Instance.GetDeathParticlesObjectFromPool();
        particleObject.transform.position = _transform.position;
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        particleSystem.startColor = _originalColor;
      
    }
}

