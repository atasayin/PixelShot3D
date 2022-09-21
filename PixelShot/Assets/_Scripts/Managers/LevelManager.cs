using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : AbstarctMonoSingleton<LevelManager>
{
    [SerializeField] private int _backIndex = 3;
    [SerializeField] private List<Level> _levels;
    [SerializeField] private List<GameObject> bonusLevels;
    [SerializeField] private BallHubBehaviour _ballHub;
    [SerializeField] private Vector3 remHealthScorePopUp;
    [SerializeField] private CanonBehaviour canonBehaviour;

    private const int ULT_PROGRESS_BOUND = 1000;
    private const int REMAIN_BALL_COEF = 100;

    private bool _isPlaying, _isWin, _isUltActive;
    private Level _level;
    private int _levelLoadNumber;
    private int _totalPixelsToHit;
    private int _currentPixelsToHit;
    private int _health;
    private int _currentRecScore;

    private int _currentLevel;
    private float _levelProgressPercentage;
    private int _currencyPlayer;
    private int _ultProgress;

    public delegate void LoadLevel();
    public static event LoadLevel OnLoadLevel;

    private GameObject _activeBall;
    private GameObject _bonusLevel;


    public void Init(int levelNumber, int currency, int recScore, int ultProgress)
    {
        _isPlaying = false;
        _currentLevel = levelNumber;
        _levelLoadNumber = _currentLevel;
        _currencyPlayer = currency;
        _currentRecScore = recScore;
        _ultProgress = ultProgress;
        SubBallEvents();
    }

    private void SubBallEvents()
    {
        DragAndShootBehaviour.OnBallThrow += OnLevelPlaying;
        Pool_BallDies.OnBallDie += DecrementHealth;
        Pool_BallDies.OnBallDie += GameCycle;
        Pool_BallDies.OnBallDie += UltChecker;
    }

    public void OnDisable()
    {
        UnSubBallEvents(); 
    }


    private void UnSubBallEvents()
    {
        DragAndShootBehaviour.OnBallThrow -= OnLevelPlaying;
        Pool_BallDies.OnBallDie -= DecrementHealth;
        Pool_BallDies.OnBallDie -= GameCycle;
        Pool_BallDies.OnBallDie -= UltChecker;
    }


    public void LoadPresentLevelToScene(int levelNumber)
    {
       
        _isPlaying = false;
        _isWin = false; 
        _levelLoadNumber = levelNumber - 1;
        _currentLevel = levelNumber;

        _levelLoadNumber = _levelLoadNumber % _levels.Count;
        
        _level = _levels[_levelLoadNumber];
        _health = _level.health;
        _totalPixelsToHit = _level.simplePixelCount;
        _currentPixelsToHit = 0;

        ObjectManager.Instance.ChangeColorOfLevelBoard(_level.levelBoardColor);
        ObjectManager.Instance.IncrementPoolsOfNeed(_level);
        _activeBall = ObjectManager.Instance.GetBallObjectFromPool();
        _activeBall.GetComponent<BallBehavior>().MakeNormalBall();
        ObjectManager.Instance.GetObjectsForLevel(_level);
        OnLoadLevel?.Invoke();

        _ballHub.SetUpBallHub(_level.ballHubColor);
        _ballHub.ArrangeBall(_activeBall);

        ObjectManager.Instance.InvokeAllAnimationBehaviours();
        UIManager.Instance.OpenBallHub();
    }


    public void DecrementHealth()
    {
        if (!_isUltActive)
        {
            _health--;
            UIManager.Instance.SetHealthText(_health);
        }
    }

    public void UltChecker()
    {
        if (_currentRecScore > ULT_PROGRESS_BOUND)
        {
            UIManager.Instance.ChangeUltSprite();
            UIManager.Instance.UltButtonAvailable();
        }
    }

    public void GameCycle()
    {

        if (_isPlaying)
        {

            _levelProgressPercentage = (_totalPixelsToHit != 0) ? (float)_currentPixelsToHit / _totalPixelsToHit : 0;

            if (!_isUltActive)
            {
                if (_levelProgressPercentage == 1)
                {
                    _isPlaying = false;
                    _isWin = true;
                    EndGameDecideNextState();
                
                    return;
                }
                else if (_health <= 0)
                {
                    _isPlaying = false;
                    _isWin = false;
                    EndGameDecideNextState();
                
                    return;
                }
                
                ManageBallHub();

            }

            

        }
    }

    public void ManageBallHub()
    {
        _activeBall = ObjectManager.Instance.GetBallObjectFromPool();
        _activeBall.GetComponent<BallBehavior>().MakeNormalBall();
        _ballHub.ArrangeBall(_activeBall);
    }

    private void EndGameDecideNextState()
    {
        if(_level.levelType == LevelTypes.NormalLevel)
        {
            if (_isWin)
            {
                StartCoroutine(GetPointsFromRemHealthCoroutine());
                GameManager.Instance.WinStateAfterPlaying();
            }
            else
            {
                GameManager.Instance.LoseStateAfterPlaying();
            }
        }
        else
        {
            if (_isWin)
            {
                GameManager.Instance.BonusLevelAfterPlaying();
            }
            else
            {
                GameManager.Instance.LoseStateAfterPlaying();
            }

        }
    }

   
    private IEnumerator GetPointsFromRemHealthCoroutine()
    {
        int remHealth = _health;
        int ballScore = _currentLevel * 100;
        for (int i = 0; i < remHealth; i++)
        {
            _currentRecScore += ballScore;
            _ultProgress += ballScore;

            UIManager.Instance.SetRecScoreText(_currentRecScore);
            UIManager.Instance.SetUltProgress(_ultProgress);
            GameObject popUp = ObjectManager.Instance.GetDeathScorePopUpObjectFromPool(ballScore);
            popUp.transform.position = remHealthScorePopUp;

            DecrementHealth();
            yield return new WaitForSeconds(0.3f);
            popUp.SetActive(false);
        }
    }

   

    public void ClearLevel()
    {
        _isPlaying = false;
        ObjectManager.Instance.ResetLoadedObjects();
    }

    public void OnLevelPlaying()
    {
        _isPlaying = true;
    }

    public void ScoreLevelNumberAmountOnHit()
    {
        _currentPixelsToHit++;
        _currentRecScore += _currentLevel;
        _ultProgress += _currentLevel;
        
        UIManager.Instance.SetRecScoreText(_currentRecScore);
        UIManager.Instance.SetUltProgress(_ultProgress);
    }

    


    public int LevelLoadNumber
    {
        get{ return _levelLoadNumber;  }
        set { _levelLoadNumber = value; }
    }

    public Level LoadedLevel
    {
        get { return _level; }
        set { _level = value; }
    }

    public List<Level> Levels
    {
        get { return _levels; }
        set { _levels = value; }
    }

    public bool IsPlaying
    {
        get { return _isPlaying; }
        set { _isPlaying = value; }
    }

    public bool IsWin
    {
        get { return _isWin; }
        set { _isWin = value; }
    }

    public bool IsUltActive
    {
        get { return _isUltActive; }
        set { _isUltActive = value; }
    }

    public int CurrentPixelsToHit
    {
        get { return _currentPixelsToHit; }
        set { _currentPixelsToHit = value; }
    }

    public int CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }

    public float LevelProgressPercentage
    {
        get { return _levelProgressPercentage; }
        set { _levelProgressPercentage = value; }
    }


    public int TotalPixelsToHit
    {
        get { return _totalPixelsToHit; }
        set { _totalPixelsToHit = value; }
    }

    public int CurrencyPlayer
    {
        get { return _currencyPlayer; }
        set { _currencyPlayer = value; }
    }

    public int CurrentRecScore
    {
        get { return _currentRecScore; }
        set { _currentRecScore = value; }
    }

    public int UltProgress
    {
        get { return _ultProgress; }
        set { _ultProgress = value; }
    }

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public GameObject ActiveBall
    {
        get { return _activeBall; }
        set { _activeBall = value; }
    }
  

}
