using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : AbstarctMonoSingleton<UIManager>
{

    #region Canvas
    [Header("Canvas")]
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _levelCanvas;
    [SerializeField] private GameObject _levelParts;
    [SerializeField] private GameObject _endGameWinCanvas;
    [SerializeField] private GameObject _endGameLoseCanvas;
    [SerializeField] private GameObject _endGameBonusCanvas;
    [SerializeField] private GameObject interstitialAd;
    [SerializeField] private CanvasGroup _canvasTranstitionFade;

    #endregion

    #region MainMenu UI
    [Header("Main Menu UI")]
    [SerializeField] private TextMeshProUGUI _currencyText;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private TextMeshProUGUI _playButtonText;
    [SerializeField] private RectTransform _title;
    [SerializeField] private Image blurMiddle;
    [SerializeField] private Image blurLS;
    [SerializeField] private Image blurRS;

    private Tween _mainTitle, _fadeIn, _fadeOut, _bonusYellowTextAnim;
  

    #endregion

    #region Level UI

    [Header("Level UI")]
    [SerializeField] private TextMeshProUGUI _currentLevelText;
    [SerializeField] private TextMeshProUGUI _nextLevelText;
    [SerializeField] private TextMeshProUGUI _currencyLevelText;
    [SerializeField] private TextMeshProUGUI _healthLevelText;
    [SerializeField] private TextMeshProUGUI _recScoreText;    
    [SerializeField] private Slider _ultSlider;
    [SerializeField] private Button ultButton;
    [SerializeField] private Image ultButtonImage;
    [SerializeField] private Sprite ultFilledTexture;
    [SerializeField] private Sprite ultNonFilledTexture;
    [SerializeField] private GameObject ultCanon;
    [SerializeField] private ProgressBar progressSlider;
    [SerializeField] private GameObject bossLevelText;
    [SerializeField] private GameObject ballHub;
    [SerializeField] private GameObject bonusYellowText;
    #endregion

    #region End Game

    [Header("End Game Win UI")]
    [SerializeField] private TextMeshProUGUI _EGW_levelNumberText;
    [SerializeField] private TextMeshProUGUI _EGW_RecScoreText;
    [SerializeField] private TextMeshProUGUI _EGW_TotalScoreText;
    [SerializeField] private Image _EGW_blur;

    [Header("End Game Lose UI")]
    [SerializeField] private TextMeshProUGUI _EGL_levelProgressText;
    [SerializeField] private TextMeshProUGUI _EGL_levelNumberText;
    [SerializeField] private TextMeshProUGUI _EGL_RecScoreText;
    [SerializeField] private TextMeshProUGUI _EGL_TotalScoreText;
    [SerializeField] private Image _EGL_blur;

    [Header("End Game Bonus UI")]
    [SerializeField] private TextMeshProUGUI _EGB_diamondScoreText;


    #endregion

    private void Start()
    {
        _mainTitle.SetAutoKill(false);
        _mainTitle = _title.DOAnchorPos(Vector2.down * 100, 5).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        _bonusYellowTextAnim = bonusYellowText.GetComponent<RectTransform>().DOAnchorPosX(2000, 3f).SetEase(Ease.InCirc).SetAutoKill(false).Pause();
        _fadeIn = _canvasTranstitionFade.DOFade(1, 0.5f).SetAutoKill(false).Pause();
        _fadeOut = _canvasTranstitionFade.DOFade(0, 0.5f).SetAutoKill(false).Pause();
    }

    public void DisableAllCanvas()
    {
        _mainMenuCanvas.SetActive(false);
        _levelCanvas.SetActive(false);
        _endGameWinCanvas.SetActive(false);
        _levelParts.SetActive(false);
        _endGameBonusCanvas.SetActive(false);
    }

    public Tween FadeInAnimation()
    {
        _canvasTranstitionFade.alpha = 0;
        _fadeIn.Rewind();
        return _fadeIn.Play();
    }

    public Tween FadeOutAnimation()
    {
        _canvasTranstitionFade.alpha = 1;
        _fadeOut.Rewind();
        return _fadeOut.Play();
    }
 
    #region MainMenu 

    public void OpenMainMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _levelParts.SetActive(false);
        PlayUpDownTitleAnimation();
        
    }

    public void SetMainMenuTexts(int currency, int totalScore, int levelNumber)
    {
        _currencyText.text = currency.ToString();
        _totalScoreText.text = totalScore.ToString();
        _playButtonText.text = levelNumber.ToString();        
    }

    public void CloseMainMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _mainTitle.Restart();
        _mainTitle.Pause();

    }

    private void PlayUpDownTitleAnimation()
    {
        _mainTitle.Play();
    }
    


    #endregion

    #region Level Playing

    public void OpenLevelCanvas()
    {
        _levelCanvas.SetActive(true);
        _levelParts.SetActive(true);

        ultButtonImage.sprite = ultNonFilledTexture;
        ultCanon.SetActive(false);
        _levelCanvas.GetComponent<CanvasAnimation>().OpenCanvasAnimation();
    }

    public void CloseLevelCanvas()
    {
         _levelCanvas.SetActive(false); 
    }


    public void SetLevelTexts(int levelNumber, int currency, int health, int recScore)
    {
        _currentLevelText.text = levelNumber.ToString();
        _nextLevelText.text = (levelNumber + 1).ToString();
        _currencyLevelText.text = currency.ToString();
        _healthLevelText.text = "x" + health.ToString();
        _recScoreText.text = recScore.ToString();
    }
    public void SetCurrencyText(int currency)
    {
        _currencyLevelText.text = currency.ToString();
    }

    public void SetLevelTexts(int levelNumber, int recScore)
    {
        _currentLevelText.text = levelNumber.ToString();
        _nextLevelText.text = (levelNumber + 1).ToString();
        _recScoreText.text = recScore.ToString();
    }


    public void SetHealthText(int health)
    {
        _healthLevelText.text = "x" + health.ToString();
    }

    public void SetRecScoreText(int recScore)
    {
        _recScoreText.text = recScore.ToString();
    }

    public void SetUltProgress(int value)
    {
        _ultSlider.value = value;
    }

    public void NormalLevelAdditions()
    {
        bossLevelText.SetActive(false);
    }

    public void OpenBallHub()
    {
        ballHub.SetActive(true);
    }

    public void BossLevelAdditions()
    {
        bossLevelText.SetActive(true);
        TextMeshProUGUI textMesh =  bossLevelText.GetComponent<TextMeshProUGUI>();
        textMesh.text = "BOSS LEVEL! X";
        textMesh.color = new Color(1 , 0.07843138f, 0.282353f);
    }
   
    public void BonusLevelAdditions()
    {
        bossLevelText.SetActive(true);
        ballHub.SetActive(false);
        TextMeshProUGUI textMesh = bossLevelText.GetComponent<TextMeshProUGUI>();
        textMesh.text = "BONUS LEVEL! X";
        textMesh.color = new Color(0.0431372549f, 0.588235294f, 0.0470588235f);

       
        
    }

    public void BonusLevelYellowAnimation()
    {
        bonusYellowText.SetActive(true);
        _bonusYellowTextAnim.Rewind();
        _bonusYellowTextAnim.Play().OnComplete(()=> { bonusYellowText.SetActive(false); });
    }


    public void FillProgressSlider()
    {
        progressSlider.FillSlider();
    }

    public void UltButtonAvailable()
    {
        ultButton.enabled = true;
    }
    public void UltButtonUnAvailable()
    {
        ultButton.enabled = false;
    }


    public void SummonCanon()
    {
        LevelManager.Instance.IsUltActive = true;
        ultCanon.SetActive(true);
        ballHub.SetActive(false);
        LevelManager.Instance.ActiveBall.SetActive(false);   
    }

    public void CloseCanon()
    {
        ultCanon.SetActive(false);
    }
   
    public void ChangeUltSprite()
    {
        ultButtonImage.sprite = ultFilledTexture;
    }

    public void IntersitialAdOn()
    {
        interstitialAd.SetActive(true);
    }


    public void IntersitialAdOff()
    {
        interstitialAd.SetActive(false);
    }


    #endregion

    #region EndGameWin

    public void OpenEndGameWinCanvas()
    {
        _endGameWinCanvas.SetActive(true);
        _endGameWinCanvas.GetComponent<CanvasAnimation>().OpenCanvasAnimation();
    }

    public void CloseEndGameWinCanvas()
    {
        _endGameWinCanvas.SetActive(false);

    }

    public void SetEndGameWinTexts(int levelNumber, int recScore, int totalScore)
    {
        _EGW_levelNumberText.text = "LEVEL" + levelNumber.ToString();
        _EGW_RecScoreText.text = recScore.ToString();
        _EGW_TotalScoreText.text = totalScore.ToString();

    }

    public void SetColorEndGameWinBlur(Color color)
    {
        float H, S, V;

        Color.RGBToHSV(color, out H, out S, out V);
        V *= 0.4f;

        Color newColor = Color.HSVToRGB(H, S, V);
        newColor.a = 0.5f;
        _EGW_blur.color = newColor;

       
        
    }

    #endregion


    #region EndGameLose

    public void OpenEndGameLoseCanvas()
    {
        _endGameLoseCanvas.SetActive(true);
        _endGameLoseCanvas.GetComponent<CanvasAnimation>().OpenCanvasAnimation();
    }

    public void CloseEndGameLoseCanvas()
    {
        _endGameLoseCanvas.SetActive(false);
    }

    public void SetEndGameLoseTexts(int progress, int levelNumber, int recScore, int totalScore)
    {
        _EGL_levelProgressText.text = progress.ToString() + "%";
        _EGL_levelNumberText.text = "LEVEL " + levelNumber.ToString();
        _EGL_RecScoreText.text = recScore.ToString();
        _EGL_TotalScoreText.text = totalScore.ToString();

    }

    public void SetColorEndGameLoseBlur(Color color)
    {
        float H, S, V;

        Color.RGBToHSV(color, out H, out S, out V);
        V *= 0.4f;

        Color newColor = Color.HSVToRGB(H, S, V);
        newColor.a = 0.5f;
        _EGL_blur.color = newColor;



    }


    #endregion

    #region End Game Bonus

    public void OpenEndGameBonusCanvas()
    {
        _endGameBonusCanvas.SetActive(true);
        _endGameBonusCanvas.GetComponent<CanvasAnimation>().OpenCanvasAnimation();
        
    }

    public void CloseEndGameBonusCanvas()
    {
        _endGameBonusCanvas.SetActive(false);
        bonusYellowText.SetActive(false);
        bonusYellowText.transform.position = new Vector2(-1600,500);
    }
  
    public void SetEndGameBonusdiamondScore(int score)
    {
        _EGB_diamondScoreText.text = score.ToString();
    }

    #endregion


}
