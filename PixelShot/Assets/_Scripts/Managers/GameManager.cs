using System.Collections;
using UnityEngine;
using DG.Tweening;
public class GameManager : AbstarctMonoSingleton<GameManager>
{
    #region Variables
    private const float STATE_TRANSITION_DURATION = 1;
    public static int DIAMOND_FOR_BONUS_LEVEL = 50;

    private StateMachine _stateMachine;
    private int _currentRecScore;
    private int _currentTotalScore;
    private int _highRecScore;
    private int _currentLevel;
    private int _currencyPlayer;
    private int _ultProgress;
    public bool isPlaying = false;

    public bool isEditor = true;
    public bool isResetPlayer;

    #endregion


    private void Start()
    {
        UIManager.Instance.DisableAllCanvas();
        if (isResetPlayer)
        {
            PlayerManager.ResetPlayerInfo();
        }
        PlayerManager.Init();
        SetUpPlayerStats();

        _stateMachine = new StateMachine();

        LevelManager.Instance.Init(_currentLevel, _currencyPlayer, _currentRecScore, _ultProgress);

        _stateMachine.ChangeState(States.MainMenu);
    }

    private void SetUpPlayerStats()
    {
        _currentLevel = PlayerManager.GetPresentLevel(); 
        _currencyPlayer = PlayerManager.GetCurrency();
        _currentTotalScore = PlayerManager.GetTotalScore();
        _currentRecScore = PlayerManager.GetRecursiveScore();
        _ultProgress = PlayerManager.GetUltimateProgress();
        _highRecScore = PlayerManager.GetHighRecursiveScore();
    }

    private void OnDisable()
    {
        SavePlayerStats();
    }

    private void SavePlayerStats()
    {
        PlayerManager.SetPresentLevel(_currentLevel);
        PlayerManager.SetCurrency(_currencyPlayer);
        PlayerManager.SetUltimateProgress(_ultProgress);
    }


    private void Update()
    {
        StateEditor();
    }

    private void StateEditor()
    {
        if (isEditor)
        {
            if (Input.GetKeyDown("q"))
            {
                GoToMainMenu();
            }

            if (Input.GetKeyDown("w"))
            {
                GoToLevelPlaying();
            }

            if (Input.GetKeyDown("e"))
            {
                GoToEndGameWin();
            }

            if (Input.GetKeyDown("space"))
            {
                GoToNextLevel();
            }
        }

    }

    public void GoToMainMenu()
    {
        UIManager.Instance.FadeInAnimation().OnComplete(() => {
            UIManager.Instance.FadeOutAnimation();
            _stateMachine.ChangeState(States.MainMenu);
        });
    }

    public void GoToLevelPlaying()
    {        
        UIManager.Instance.FadeInAnimation().OnComplete( () => {
            UIManager.Instance.FadeOutAnimation();
            _stateMachine.ChangeState(States.LevelPlaying);
        });          
    }

   
    public void GoToEndGameWin()
    {
        _stateMachine.ChangeState(States.EndGameWin);
        _currentLevel++;
        SavePlayerStats();
    }

    public void WinStateAfterPlaying()
    {
        StartCoroutine(WinStateCoroutine());
    }

    private IEnumerator WinStateCoroutine()
    {
        yield return new WaitForSeconds(2);
        GoToEndGameWin();
    }


    public void GoToEndGameLose()
    {
        _stateMachine.ChangeState(States.EndGameLose);
    }

    public void LoseStateAfterPlaying()
    {
        StartCoroutine(LoseStateCoroutine());
    }

    private IEnumerator LoseStateCoroutine()
    {
        yield return new WaitForSeconds(2);
        GoToEndGameLose();
    }


    public void GoToEndGameBonus()
    {
        StartCoroutine(GoToEndGameBonusCoroutine());
    }

    private IEnumerator GoToEndGameBonusCoroutine()
    {
        yield return new WaitForSeconds(4);
        _stateMachine.ChangeState(States.EndGameBonus);
        _currentLevel++;
        SavePlayerStats();
    }

    public void GoToBonusLevelPlaying()
    {
        _stateMachine.ChangeState(States.BonusLevelPlaying);
    }

    private void GoToNextLevel()
    {
        _currentLevel++;
        _stateMachine.ChangeState(States.LevelPlaying);
    }

    

    

    public void BonusLevelAfterPlaying()
    {
        StartCoroutine(BonusLevelCoroutine());
    }

    private IEnumerator BonusLevelCoroutine()
    {
        yield return new WaitForSeconds(2);
        GoToBonusLevelPlaying();
    }





    #region Getter Setter

    public StateMachine StateMachine
    {
        get
        {
            return _stateMachine;
        }
    }

    public int CurrentLevel
    {
        get { return _currentLevel;  }
        set { _currentLevel = value; }
    }

    public int CurrencyPlayer
    {
        get { return _currencyPlayer; }
        set { _currencyPlayer = value; }
    }

    public int RecScore
    {
        get { return _currentRecScore; }
        set { _currentRecScore = value; }
    }

    public int TotalScore
    {
        get { return _currentTotalScore; }
        set { _currentTotalScore = value; }
    }
    public int HighRecScore
    {
        get { return _highRecScore; }
        set { _highRecScore = value; }
    }


    
    #endregion


}


