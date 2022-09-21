
public class BonusLevelPlayingState : AbstractStateBase
{
    
    public override void EnterState()
    {
        UIManager.Instance.BonusLevelYellowAnimation();

        int levelNumber = GameManager.Instance.CurrentLevel;
        int currency = GameManager.Instance.CurrencyPlayer;
        int recScore = LevelManager.Instance.CurrentRecScore;


        ObjectManager.Instance.GetPinyataObjectFromPool();
        int health = LevelManager.Instance.Health;

        UIManager.Instance.OpenLevelCanvas();
        UIManager.Instance.FillProgressSlider();
        UIManager.Instance.BonusLevelAdditions();
        UIManager.Instance.SetLevelTexts(levelNumber, currency, health, recScore);
    }

    public override void ExitState()
    {
        LevelManager.Instance.ClearLevel();
        UIManager.Instance.CloseLevelCanvas();
        
    }
}

