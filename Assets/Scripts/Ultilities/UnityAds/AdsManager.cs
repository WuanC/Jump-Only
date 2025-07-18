using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    [field: SerializeField] public AdsInitializer AdsInitializer { get; private set; }
    [field: SerializeField] public InterstitialAds InterstitialAds { get; private set; }
    [field: SerializeField] public BannerAds BannerAds { get; private set; }
    [field: SerializeField] public RewardedAds RewardedAds { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }
}
