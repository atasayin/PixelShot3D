using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        GameManager.Instance.GoToLevelPlaying();                   
    } 
}
