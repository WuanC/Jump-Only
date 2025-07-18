using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public EGameMode gameMode;

    [SerializeField] float timeLoadNewScene;


    private GameObject currentLevelObj;
    [SerializeField] private int currentLevel = 1;
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
        }
    }




    [Header("Player Spawn Endless")]
    [SerializeField] float radiusCheckObstacle;
    Player player;




    //Event
    public event Action<string, int> OnLevelChanged;
    public event Action OnClearLevel;
    public int IdSkinSelected { get; set; }

    private void Start()
    {
       StartCoroutine(ShowBannerAds());
    }
    IEnumerator ShowBannerAds()
    {
        yield return new WaitForSeconds(1f);
        AdsManager.Instance.BannerAds.ShowBannerAds();
    }

    #region level manager
    public void SlowMotionWin()
    {
        StartCoroutine(PlayerAdventureWinCouroutine());
    }
    public IEnumerator PlayerAdventureWinCouroutine()
    {
        Time.timeScale = 0.5f;
        OnClearLevel?.Invoke();
        yield return new WaitForSeconds(timeLoadNewScene);
        Time.timeScale = 1f;
        Observer.Instance.Broadcast(EventId.OnPlayerWin, null);
    }
    public void NextLevelAdventure()
    {
        if (currentLevel == DatabaseManager.Instance.LevelDatas.Count)
        {
            Observer.Instance.Broadcast(EventId.OnPlayerCompletedGame, null);
            Destroy(currentLevelObj.gameObject);
            return;
        }
        currentLevel++;
        Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() => LoadNewLevel(currentLevel.ToString())));
    }



    public void DeleteCurrentLevel()
    {
        if(currentLevelObj != null)
        {
            Destroy(currentLevelObj.gameObject);
        }
    }
    public void RestartCurrentLevel()
    {
        DeleteCurrentLevel();
        if (gameMode == EGameMode.Adventure)
            Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() => LoadNewLevel(currentLevel.ToString())));
        else if (gameMode == EGameMode.Endless)
            Observer.Instance.Broadcast(EventId.OnTransitionScreen, (Action)(() => LoadEndlessLevel()));
    }
    public void LoadNewLevel(string level = "1")
    {
        if (HeartManager.Instance.IsRemainingHearts())
        {
            AdsManager.Instance.InterstitialAds.ShowAd();
            gameMode = EGameMode.Adventure;
            HeartManager.Instance.UseHeart();
            if (currentLevelObj != null) Destroy(currentLevelObj);
            SAVE.SaveLevel(level);
            currentLevelObj = Instantiate(DatabaseManager.Instance.LevelDatas[level].levelPrefabs);
            OnLevelChanged?.Invoke(level, DatabaseManager.Instance.LevelDatas.Count);
        }
        else
        {
            Debug.LogError("Not enough hearts");
            DeleteCurrentLevel();
            Observer.Instance.Broadcast(EventId.OnBackToMenu, null);
        }

    }
    public void LoadEndlessLevel()
    {
        gameMode = EGameMode.Endless;
        if (currentLevelObj != null) Destroy(currentLevelObj);
        currentLevelObj = Instantiate(DatabaseManager.Instance.LevelEndlessPrefabs);
        AdsManager.Instance.InterstitialAds.ShowAd();
    }
    #endregion

    #region spawn endless

    public void RespawnEndless(Player player)
    {
        this.player = player;
    }
    public IEnumerator SetPosGOInRange(Vector2 pointOrigin, float radiusCheck, Transform gameObj, Transform[] lockXArr)
    {
        Vector2 newPos = gameObj.position;
        while (true)
        {
            int randomIndex = UnityEngine.Random.Range(0, lockXArr.Length);
            float xPos = lockXArr[randomIndex].position.x;
            float yPos = (pointOrigin + UnityEngine.Random.insideUnitCircle * radiusCheck).y;
            newPos = new Vector2(xPos, yPos);
            Collider2D hit = Physics2D.OverlapPoint(newPos);
            if (hit == null)
            {
                break;
            }
            yield return null;
        }
        gameObj.position = newPos;
        
    }

    public void ContinueEndlessMode()
    {
        if(player != null) player.RespawnEndless();
    }
    public void RestartEndlessMode()
    {
        LoadEndlessLevel();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere((Vector2)Camera.main.transform.position, radiusCheckObstacle);
    }


    #endregion

}
public enum EGameMode
{
    Endless,
    Adventure,
}
