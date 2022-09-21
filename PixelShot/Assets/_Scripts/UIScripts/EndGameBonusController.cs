using UnityEngine;

public class EndGameBonusController : MonoBehaviour
{
    public void OnClickNextLevelButton()
    {
        GameManager.Instance.GoToLevelPlaying();
    }


    public void OnClickHomeButton()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
