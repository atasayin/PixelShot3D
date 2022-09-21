using System;
using UnityEngine;


public class LevelEndWinState : AbstractStateBase
{

    public override void EnterState()
    {
        int levelNumber = GameManager.Instance.CurrentLevel;
        int recScore = LevelManager.Instance.CurrentRecScore;
        GameManager.Instance.TotalScore += LevelManager.Instance.TotalPixelsToHit * levelNumber;
        Color levelBoardColor = LevelManager.Instance.LoadedLevel.levelBoardColor;

        int totalScore = GameManager.Instance.TotalScore;

        PlayerManager.SetRecursiveScore(recScore);
        PlayerManager.SetTotalScore(totalScore);

        UIManager.Instance.OpenEndGameWinCanvas();
        UIManager.Instance.SetEndGameWinTexts(levelNumber, recScore, totalScore);
        UIManager.Instance.SetColorEndGameWinBlur(levelBoardColor);
        AudioManager.Instance.EndGameWin();
    }

    public override void ExitState()
    {
        UIManager.Instance.CloseEndGameWinCanvas();
        AdsManager.LoadInterstitiaAd();
    }
}