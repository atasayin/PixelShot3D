using UnityEngine;
using UnityEngine.Advertisements;


public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{

    [SerializeField] private string playstore_id;
    [SerializeField] private string appstore_id;

    [SerializeField] private bool _testMode = true;

    private string _gameId;


    private static string _androidInterstitialAdUnitId = "Interstitial_Android";
    private static string _iOsAdInterstitialUnitId = "Interstitial_iOS";
    private static string _adInterstitialUnitId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer){
            _gameId = appstore_id;
            _adInterstitialUnitId = _iOsAdInterstitialUnitId;
        }
        else
        {
            _gameId = playstore_id;
            _adInterstitialUnitId = _androidInterstitialAdUnitId;
        }
            
        Advertisement.Initialize(_gameId, _testMode, this);

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }


    public static void LoadInterstitiaAd()
    {
        Advertisement.Load(_adInterstitialUnitId);
        Advertisement.Show(_adInterstitialUnitId);
    }


}
