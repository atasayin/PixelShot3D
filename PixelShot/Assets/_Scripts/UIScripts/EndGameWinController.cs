using UnityEngine;

public class EndGameWinController : MonoBehaviour
{

    public void OnClickNextLevelButton()
    {
        GameManager.Instance.GoToLevelPlaying();
    }

    public void OnClickDailySpinButton()
    {
        Debug.Log("DailySpinButton");
    }

    public void OnClickHomeButton()
    {
        GameManager.Instance.GoToMainMenu();
    }



}
