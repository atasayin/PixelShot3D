using UnityEngine;

public class EndGameLoseController : MonoBehaviour
{

    public void OnClickRestartButton()
    {
        GameManager.Instance.GoToLevelPlaying();
    }


    public void OnClickHomeButton()
    {
        GameManager.Instance.GoToMainMenu();
    }


}
