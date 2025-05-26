using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public EGameMode gameMode;

    [SerializeField] float timeLoadNewScene;

    [Header("Level mode")]
    private const string levelPath = "Levels";
    private Dictionary<string, LevelSO> levelDatas = new();
    public Dictionary<string, LevelSO> Levels => levelDatas;

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


    [Header("Endless mode")]
    private const string endlessPath = "LevelEndless";
    public GameObject levelEndlessPrefabs;



    [Header("Player Spawn Endless")]
    [SerializeField] float radiusCheckObstacle;
    Player player;


    [Header("Currency")]
    [SerializeField] int coins;
    [SerializeField] int hearts;
    public int Coins => coins;
    public int Hearts => hearts;


    //Event
    public event Action<string, int> OnLevelChanged;
    public event Action OnClearLevel;
    public int IdSkinSelected { get; set; }
    protected override void Awake()
    {
        base.Awake();
        LoadData();
        //coins = SAVE.GetCoins();
        //hearts = SAVE.GetHearts();
    }
    public void LoadData()
    {
        List<LevelSO> levelSOs = Resources.LoadAll<LevelSO>(levelPath).ToList();
        levelDatas = levelSOs.ToDictionary(level => level.level);

        List<GameObject> list = Resources.LoadAll<GameObject>(endlessPath).ToList();
        levelEndlessPrefabs = list.First();
    }

    #region level manager

    public void PlayerWin()
    {
        StartCoroutine(PlayerWinCouroutine());
    }
    public IEnumerator PlayerWinCouroutine()
    {
        Time.timeScale = 0.3f;
        OnClearLevel?.Invoke();
        yield return new WaitForSeconds(timeLoadNewScene);
        Time.timeScale = 1f;

        if (currentLevel == levelDatas.Count)
        {
            Observer.Instance.Broadcast(EventId.OnPlayerCompletedGame, null);
            Destroy(currentLevelObj.gameObject);
            yield break; //do sth
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
    public void LoadNewLevel(string level = "1")
    {
        if (WithdrawHearts(1))
        {
            gameMode = EGameMode.Adventure;
            if (currentLevelObj != null) Destroy(currentLevelObj);
            SAVE.SaveLevel(level);
            currentLevelObj = Instantiate(levelDatas[level].levelPrefabs);
            OnLevelChanged?.Invoke(level, levelDatas.Count);
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
        currentLevelObj = Instantiate(levelEndlessPrefabs);
    }
    #endregion

    #region spawn endless

    private Vector2 tmpPos;
    public void RespawnEndless(Player player)
    {
        this.player = player;
        StartCoroutine(SetPosGOInRange((Vector2)Camera.main.transform.position ,radiusCheckObstacle, player.transform));
    }
    public IEnumerator SetPosGOInRange(Vector2 pointOrigin,float radiusCheck, Transform gameObj)
    {
        Vector2 newPos = gameObj.position;
        while (true)
        {
            newPos = pointOrigin + UnityEngine.Random.insideUnitCircle * radiusCheck;

            //RaycastHit hit;
            //Physics.Raycast(Camera.main.transform.position, new Vector3(newPos.x, newPos.y, 0) - Camera.main.transform.position,out hit, Mathf.Infinity);
           Collider2D hit = Physics2D.OverlapPoint(newPos);
            if (hit == null)
            {
                break;
            }
            else
            {
                tmpPos = newPos;
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
        Gizmos.DrawRay(Camera.main.transform.position, new Vector3(tmpPos.x, tmpPos.y, 0) - Camera.main.transform.position);
        Gizmos.DrawWireSphere((Vector2)Camera.main.transform.position, radiusCheckObstacle);
    }


    #endregion

    #region currency

    public bool WithdrawCoins(int coinsWithdraw)
    {
        if(coins - coinsWithdraw >= 0)
        {
            coins = coins - coinsWithdraw;
            Observer.Instance.Broadcast(EventId.OnUpdateCoins, coins);
            SAVE.SaveCoins(coins);
            return true;
        }
        return false;
    }
    public void DepositeCoins(int coinsDeposite)
    {
        coins += coinsDeposite;
        Observer.Instance.Broadcast(EventId.OnUpdateCoins, coins);
        SAVE.SaveCoins(coins);
    }
    public bool WithdrawHearts(int heartsWithdraw)
    {
        if (hearts - heartsWithdraw >= 0)
        {
            hearts = hearts - heartsWithdraw;
            Observer.Instance.Broadcast(EventId.OnUpdateHearts, hearts);
            SAVE.SaveHearts(hearts);
            return true;
        }
        return false;
    }

    #endregion
}
public enum EGameMode
{
    Endless,
    Adventure,
}
