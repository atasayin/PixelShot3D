using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : AbstarctMonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private AudioSource ballHubFirstTouch;
    [SerializeField] private AudioSource ballTouchWall;
    [SerializeField] private AudioSource endGameWinShin;
    [SerializeField] private AudioSource endGameLose;

    public void PlayButtonClick()
    {
        buttonClick.Play();
    }


    public void BallHubFirstTouch()
    {
        ballHubFirstTouch.Play();
    }

    public void BallTouchWallTouch()
    {
        ballTouchWall.Play();
    }
    public void EndGameWin()
    {
        endGameWinShin.Play();
    }


    public void EndGameLose()
    {
        endGameLose.Play();
    }

}
