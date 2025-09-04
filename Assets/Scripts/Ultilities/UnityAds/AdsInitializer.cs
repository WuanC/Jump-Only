using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;

    [SerializeField] InterstitialAds interstitialAds;
    [SerializeField] BannerAds bannerAds;
    [SerializeField] RewardedAds rewardedAds;
    private string _gameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        #if UNITY_IOS
                _gameId = _iOSGameId;
        #elif UNITY_ANDROID
                _gameId = _androidGameId;
        #elif UNITY_EDITOR
                _gameId = _androidGameId; //Only for testing the functionality in the Editor
        #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        Debug.Log("Init");
    }


    public void OnInitializationComplete()
    {
        interstitialAds.LoadAd();
        bannerAds.LoadBannerAds();
        rewardedAds.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
