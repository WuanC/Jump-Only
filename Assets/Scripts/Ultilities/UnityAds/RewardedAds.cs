using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId;
    [SerializeField] bool isReady;
    public event Action<UnityAdsShowCompletionState> OnAdCompleted;

    private void Awake()
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
            Advertisement.Show(_adUnitId, this);
            isReady = false;
            LoadAd();
        }
    }
    #region Load callback
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == _adUnitId)
        {
            isReady = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }
    #endregion

    #region Show callback
    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId != _adUnitId) return;
        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {

        }
        else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        {
            
        }
        else if (showCompletionState == UnityAdsShowCompletionState.UNKNOWN)
        {
            // Handle unknown state
        }
        OnAdCompleted?.Invoke(showCompletionState);


        AudioManager.Instance.ToggleVolumeAll(true);
        Time.timeScale = 1;
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        if (this._adUnitId != placementId) return;
        AudioManager.Instance.ToggleVolumeAll(false);
        Time.timeScale = 0;
    }
    #endregion

}
