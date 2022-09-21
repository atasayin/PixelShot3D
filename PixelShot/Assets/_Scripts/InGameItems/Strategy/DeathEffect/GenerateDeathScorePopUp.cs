using UnityEngine;
using DG.Tweening;
using TMPro;

public class GenerateDeathScorePopUp : IDeathEffect
{
    private Transform _transform;
    private const float POP_Z_LOWER = 0.02f;
    private const float POP_Z_UPPER = 0.2f;
    private const float DURATION = 1f;
    private Color _color;
    private bool IsDepentOnLevelNumber;

    public GenerateDeathScorePopUp(Transform transform)
    {
        _transform = transform;
        _color = new Color(0.9490197f, 0.8745099f, 0.7176471f);
        IsDepentOnLevelNumber = true;
    }

    public GenerateDeathScorePopUp(Transform transform, Color color)
    {
        _transform = transform;
        _color = color;
        IsDepentOnLevelNumber = false;
    }

    public void InvokeDeathEffect()
    {
        int levelNumber;
        if (IsDepentOnLevelNumber)
        {
            levelNumber = LevelManager.Instance.CurrentLevel;
        }
        else
        {
            levelNumber = 1;
        }

        GameObject popUp = ObjectManager.Instance.GetDeathScorePopUpObjectFromPool(levelNumber);

        popUp.GetComponent<TextMeshPro>().color = _color;
        popUp.transform.position = _transform.position;
        float goUp = GetRandomHeight();

        popUp.transform.DOMoveZ(goUp, DURATION).SetEase(Ease.OutBounce).OnComplete(
            () =>{ popUp.SetActive(false); }
            );
        
    }

    private float GetRandomHeight()
    {
        return UnityEngine.Random.Range(POP_Z_LOWER, POP_Z_UPPER);
    }
    
}

