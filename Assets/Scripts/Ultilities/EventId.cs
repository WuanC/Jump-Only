public enum EventId
{
    OnPlayerDied,
    OnPlayerRespawn,
    OnPlayerColliding,
    OnPlayerJump,                   // Quest Jump
    OnPlayerWin,

    OnTransitionScreen,
    OnPlayerCompletedGame,
    OnBackToMenu,
    OnMuteAudio,
    OnUserInput,

    //Endless Mode
    OnUpdateSpeed,
    OnEnterJumpPad,
    OnBroadcastSpeed,

    //Boost
    OnAddBoost,
    OnRemoveBoost,
    OnUpdateBoost,
    OnPickupMagnetCoins,

    //Skin
    OnSelectedSkin,


    //Currency,
    OnUpdateCoins,
    OnUpdateHearts,

    //Collect Gift
    OnCollectGift,


    //Quest
    OnPickupBoost,
    OnSpendCoins,

}
