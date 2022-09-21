using System.Collections;
using UnityEngine;


public class LevelPlayingState : AbstractStateBase
{
    private LevelTypes _levelTypes;
      
    public override void EnterState()
    {
        int levelNumber = GameManager.Instance.CurrentLevel;
        int currency = GameManager.Instance.CurrencyPlayer;
        int recScore = LevelManager.Instance.CurrentRecScore;
        
        LevelManager.Instance.LoadPresentLevelToScene(levelNumber);
        Level loadedLevel = LevelManager.Instance.LoadedLevel;
        int health = loadedLevel.health;
        _levelTypes = loadedLevel.levelType;
        
        UIManager.Instance.OpenLevelCanvas();
        UIManager.Instance.SetLevelTexts(levelNumber, currency, health, recScore);

        if (_levelTypes == LevelTypes.NormalLevel)
        {
            UIManager.Instance.NormalLevelAdditions();
        }
        else
        {
            UIManager.Instance.BossLevelAdditions();
        }

    }

    public override void ExitState()
    {
        ObjectManager.Instance.FinishAllAnimationBehaviours();

        if (_levelTypes == LevelTypes.BossLevel)
        {
            ObjectManager.Instance.ReturnShinEffectObjectToPool();            
        }
       

        UIManager.Instance.CloseLevelCanvas();
        LevelManager.Instance.ClearLevel();

        
        

        
    }

    

}
