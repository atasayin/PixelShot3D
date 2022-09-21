using System;
using UnityEngine;


public class MainMenuState : AbstractStateBase
{
    public override void EnterState()
    {

        int totalScore = PlayerManager.GetTotalScore();
        int currency = PlayerManager.GetCurrency();
        int levelNumber = PlayerManager.GetPresentLevel();

        UIManager.Instance.OpenMainMenu();

        UIManager.Instance.SetMainMenuTexts(currency, totalScore, levelNumber);


    }



    public override void ExitState()
    {
        UIManager.Instance.CloseMainMenu();
        
    }
}
