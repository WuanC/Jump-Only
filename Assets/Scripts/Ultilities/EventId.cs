public enum EventId
{
    OnPlayerDied,
    OnPlayerRespawn,
    OnPlayerColliding,
    OnPlayerJump,                   // Quest Jump
    OnPlayerWin,

    //SaveLoad
    OnUnlockNewLevel,


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

    //Effect
    OnSpawnEffect,

    //ChangeMap
    OnPlayerLoseInAdventure,

}
