using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake()
    {
        adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iosAdUnitId
            : androidAdUnitId;

        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }
    public void LoadBannerAds()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load(adUnitId, options);
    }
    #region Banner Load Callback
    private void OnBannerError(string message)
    {
    }

    private void OnBannerLoaded()
    {
    }
    #endregion
    public void ShowBannerAds()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = OnBannerShown,
            hideCallback = OnBannerHidden
        };
        Advertisement.Banner.Show(adUnitId, options);
    }
    #region Banner Show Callback
    private void OnBannerHidden()
    {
    }

    private void OnBannerShown()
    {
    }
    #endregion
    public void HideBannerAds()
    {
        Advertisement.Banner.Hide();
    }
}
