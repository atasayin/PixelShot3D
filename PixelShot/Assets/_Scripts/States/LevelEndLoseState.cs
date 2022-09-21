using UnityEngine;


public class LevelEndLoseState : AbstractStateBase
{

    public override void EnterState()
    {
        int progress = Mathf.FloorToInt(100 * LevelManager.Instance.LevelProgressPercentage);
        int levelNumber = GameManager.Instance.CurrentLevel;
        int highScore = GameManager.Instance.HighRecScore;
        int recScore = LevelManager.Instance.CurrentRecScore;
        int totalScore = GameManager.Instance.TotalScore;
        Color levelBoardColor = LevelManager.Instance.LoadedLevel.levelBoardColor;

        
        if (highScore < recScore)
        {
            // HighScore PopUp
            PlayerManager.SetHighRecursiveScorel(recScore);
        }
        GameManager.Instance.RecScore = 0;
        LevelManager.Instance.CurrentRecScore = 0;
        PlayerManager.SetRecursiveScore(0);


        UIManager.Instance.OpenEndGameLoseCanvas();
        UIManager.Instance.SetEndGameLoseTexts(progress, levelNumber,recScore, totalScore);
        UIManager.Instance.SetColorEndGameLoseBlur(levelBoardColor);
    }

    public override void ExitState()
    {
        UIManager.Instance.CloseEndGameLoseCanvas();
        AdsManager.LoadInterstitiaAd();
    }
}