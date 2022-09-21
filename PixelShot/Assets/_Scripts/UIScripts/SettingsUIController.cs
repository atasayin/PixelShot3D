using UnityEngine;
using DG.Tweening;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsPanelVibButton;
    [SerializeField] private GameObject settingsPanelSoundButton;
    [SerializeField] private GameObject settingsPanelNoAdsButton;


    private bool isOpenSettinsPanel;
    private const float SETTINGS_BUTTON_DURATION = 0.5f;
    private const float CHILD_BUTTON_DURATIONS = 0.2f;

    public void ChangeSettingsPanelState()
    {
        float rotationZ, buttonScale;
        Vector3 initVector = Vector3.zero;

        if (isOpenSettinsPanel)
        {
            rotationZ = 0;
            buttonScale = 0;
            initVector = Vector3.one;
        }
        else
        {
            settingsPanel.SetActive(true);
            rotationZ = 90f;
            buttonScale = 1;
            initVector = Vector3.zero;
        }


        settingsPanelVibButton.transform.localScale = initVector;
        settingsPanelSoundButton.transform.localScale = initVector;
        settingsPanelNoAdsButton.transform.localScale = initVector;

        settingsButton.transform.DORotate(new Vector3(0, 0, rotationZ), SETTINGS_BUTTON_DURATION);
        ChildAnimations(buttonScale).OnComplete(() => settingsPanel.SetActive(isOpenSettinsPanel));

        isOpenSettinsPanel = !isOpenSettinsPanel;
    }


    private Tween ChildAnimations(float buttonScale)
    {
        var sequence = DOTween.Sequence();

        sequence.Append(settingsPanelVibButton.transform.DOScale(buttonScale, CHILD_BUTTON_DURATIONS).SetEase(Ease.OutBounce));
        sequence.Append(settingsPanelSoundButton.transform.DOScale(buttonScale, CHILD_BUTTON_DURATIONS).SetEase(Ease.OutBounce));
        sequence.Append(settingsPanelNoAdsButton.transform.DOScale(buttonScale, CHILD_BUTTON_DURATIONS).SetEase(Ease.OutBounce));
        return sequence;
    }
}
