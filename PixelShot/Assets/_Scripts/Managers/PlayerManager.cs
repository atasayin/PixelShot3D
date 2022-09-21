using UnityEngine;


public class PlayerManager
{

    private const string LAST_LEVEL_KEY = "LAST_LEVEL";
    private const string CURRENCY_KEY = "CURRENCY";
    private const string TOTAL_SCORE_KEY = "TS";
    private const string RECURSIVE_SCORE_KEY = "RS";
    private const string HIGH_RECURSIVE_SCORE_KEY = "HRS";
    private const string ULTIMATE_KEY = "ULT";

    private const int DEFAULT_VALUE = -1;

    public static void Init()
    {
        if (PlayerPrefs.GetInt(LAST_LEVEL_KEY, DEFAULT_VALUE) == DEFAULT_VALUE)
        {
            SetPresentLevel(1);
            SetCurrency(0);
            SetTotalScore(0);
            SetRecursiveScore(0);
            SetUltimateProgress(0);
        }
    }

    public static void ResetPlayerInfo()
    {
        PlayerPrefs.DeleteAll();
    }


    #region Getter

    public static int GetPresentLevel()
    {
        return PlayerPrefs.GetInt(LAST_LEVEL_KEY);
    }

    public static int GetCurrency()
    {
        return PlayerPrefs.GetInt(CURRENCY_KEY); 
    }

    public static int GetTotalScore()
    {
        return PlayerPrefs.GetInt(TOTAL_SCORE_KEY);
    }

    public static int GetRecursiveScore()
    {
        return PlayerPrefs.GetInt(HIGH_RECURSIVE_SCORE_KEY);
    }

    public static int GetHighRecursiveScore()
    {
        return PlayerPrefs.GetInt(RECURSIVE_SCORE_KEY);
    }

    public static int GetUltimateProgress()
    {
        return PlayerPrefs.GetInt(ULTIMATE_KEY);
    }

    #endregion

    #region Setter

    public static void SetPresentLevel(int levelNumber)
    {
        PlayerPrefs.SetInt(LAST_LEVEL_KEY, levelNumber);
    }

    public static void SetCurrency(int currency)
    {
        PlayerPrefs.SetInt(CURRENCY_KEY, currency);
    }

    public static void SetTotalScore(int totalScore)
    {
        PlayerPrefs.SetInt(TOTAL_SCORE_KEY, totalScore);
    }

    public static void SetRecursiveScore(int score)
    {
        PlayerPrefs.SetInt(RECURSIVE_SCORE_KEY, score);
    }

    public static void SetHighRecursiveScorel(int highScore)
    {
        PlayerPrefs.SetInt(HIGH_RECURSIVE_SCORE_KEY, highScore);
    }

    public static void SetUltimateProgress(int ult)
    {
        PlayerPrefs.SetInt(ULTIMATE_KEY, ult);
    }

    #endregion

}
