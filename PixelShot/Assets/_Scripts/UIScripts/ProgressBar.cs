using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;  
    [SerializeField] private Image nextLevelImage;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private Color completedImageColor;
    [SerializeField] private Color incompletedTextColor;
    
    private float _singleUnitProgress;
    
    private void OnEnable()
    {
        slider.value = 0;
        nextLevelImage.color = Color.white;
        nextLevelText.color = incompletedTextColor;
        _singleUnitProgress = 1 / (float) LevelManager.Instance.TotalPixelsToHit;
        ScorablePixelCountProgress.OnPixelDies += ProgressSlider;
    }

    private void OnDisable()
    {
        ScorablePixelCountProgress.OnPixelDies -= ProgressSlider;
    }

   
    private void ProgressSlider()
    {
        StartCoroutine(ProgressSliderCoroutine(0.2f)); // dotween

        if (slider.value >= 1 - _singleUnitProgress / 2)
        {
            FillSlider();
        }
    }

    public void FillSlider()
    {
        nextLevelImage.color = completedImageColor;
        nextLevelText.color = Color.white;
        slider.value = 1;
    }

    private IEnumerator ProgressSliderCoroutine(float animationSeconds)
    {        
        var wait = new WaitForSeconds(animationSeconds);
        slider.value += _singleUnitProgress;
        yield return wait;
    }

}
