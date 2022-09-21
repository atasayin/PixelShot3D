using UnityEngine;

public class ScoreDiamond : IDeathEffect
{
    public void InvokeDeathEffect()
    {
        int currency = GameManager.Instance.CurrencyPlayer;
        UIManager.Instance.SetCurrencyText(++currency);
        GameManager.Instance.CurrencyPlayer = currency;

    }
}
