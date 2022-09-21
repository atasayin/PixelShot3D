using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class AnimationBehaviour : MonoBehaviour
{
    [SerializeField] public AnimationType animationType;
    [SerializeField] public List<Vector3> animationPoints;
    [Range(0, 5f)] public float animationDuration;
    [SerializeField] public Vector3 animationParentLocation;
    [SerializeField] public LoopType loopType;
    [SerializeField] public Ease easeType;

    private Tween _tween;

    public void Invoke()
    {
        if (animationType == AnimationType.Rotate)
        {
            _tween = DORotateParent();
        }
        else
        {
            _tween = DOPathParent();
        }
    }

    public void CompleteAnimation()
    {
        _tween.Kill();
    }



    private Tween DOPathParent()
    {
        return transform.DOPath(animationPoints.ToArray(), animationDuration).SetLoops(-1, loopType).SetEase(easeType);
    }


    private Tween DORotateParent()
    {   
        return transform.DORotate(animationPoints[0], animationDuration, RotateMode.FastBeyond360).SetLoops(-1, loopType).SetEase(easeType);
    }


}

    

