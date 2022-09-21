using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasAnimation : MonoBehaviour
{
    [SerializeField] private List<RectTransform> canvasObjects;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float openScale = 1f;
    [SerializeField] private float closeScale = 0f;


    private Sequence _openSequence;
    private Sequence _closeSequence;

    

    private void Awake()
    {
        _openSequence = DOTween.Sequence().SetAutoKill(false).Pause();
        _closeSequence = DOTween.Sequence().SetAutoKill(false).Pause();

      
        _openSequence.AppendInterval(0.5f);
        _closeSequence.AppendInterval(0.5f);

        foreach (RectTransform transform in canvasObjects)
        {
            _openSequence.Append(transform.DOScale(openScale, animationDuration).SetEase(Ease.OutElastic));
            _closeSequence.Append(transform.DOScale(closeScale, animationDuration).SetEase(Ease.InElastic));
        }
        


    }
 
    public Sequence OpenCanvasAnimation()
    {
        foreach (RectTransform transform in canvasObjects)
        {
            transform.localScale = Vector2.zero;

        }
        _openSequence.Rewind();
        return _openSequence.Play();
        
    }


    public Sequence CloseCanvasAnimation()
    {
        foreach (RectTransform transform in canvasObjects)
        {
            transform.localScale = Vector2.one;
        }

        _closeSequence.Rewind();
        return _closeSequence.Play();
    }

}
