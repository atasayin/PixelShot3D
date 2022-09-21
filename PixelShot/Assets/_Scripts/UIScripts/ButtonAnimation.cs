using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class ButtonAnimation : MonoBehaviour
{
    private Tween _animation;
    [SerializeField] private RectTransform _transform;

    private void Awake()
    {
        _animation = _transform.DOAnchorPosY(-10f, 0.5f).SetAutoKill(false).Pause();

        
    }

    public void PlayPressedAnimation()
    {
        _animation.Restart();
    }

}
