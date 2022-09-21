using System;
using UnityEngine;


public class LevelEndBonusState : AbstractStateBase
{

    public override void EnterState()
    {         
        PlayerManager.SetRecursiveScore(GameManager.Instance.CurrencyPlayer);
        
        UIManager.Instance.OpenEndGameBonusCanvas();
        UIManager.Instance.SetEndGameBonusdiamondScore(50);
        AudioManager.Instance.EndGameWin();

    }

    public override void ExitState()
    {
        UIManager.Instance.CloseEndGameBonusCanvas();
        AdsManager.LoadInterstitiaAd();

    }
}