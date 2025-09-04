using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOSAdUnitId = "Interstitial_iOS";
    string _adUnitId;
    [SerializeField] bool isReady;
    void Awake()
    {
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
        ? _iOSAdUnitId
        : _androidAdUnitId;
    }

    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        if (isReady)
        {
            isReady = false; 
            Advertisement.Show(_adUnitId, this);
            LoadAd();
        }

    }

    #region Load Callback
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if(adUnitId == _adUnitId)
        {
            isReady = true;
        }
    }
    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
    }
    #endregion

    #region Show Callback
    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string _adUnitId) {
        if(this._adUnitId != _adUnitId) return;
        AudioManager.Instance.ToggleVolumeAll(false);
        Time.timeScale = 0;
    
    }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) {
        if (this._adUnitId != _adUnitId) return;
        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            //Debug.Log("completed");
        }
        else if(showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        {
            Debug.Log("skipped");
        }
        else if(showCompletionState == UnityAdsShowCompletionState.UNKNOWN)
        {
            Debug.Log("Ad did not complete due to an unknown error.");
        }
        AudioManager.Instance.ToggleVolumeAll(true);
        Time.timeScale = 1;
    }

    #endregion

}
