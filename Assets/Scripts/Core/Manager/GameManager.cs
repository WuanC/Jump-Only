using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] ItemDataSO coinsData;
    [SerializeField] ItemDataSO heartsData;

    Item coins;
    Item hearts;
    

    //[SerializeField] int coins;
    //[SerializeField] int hearts;
    public int Coins => coins.quantity;
    public int Hearts => hearts.quantity;


    //Event
    public event Action<string, int> OnLevelChanged;
    public event Action OnClearLevel;
    public int IdSkinSelected { get; set; }
    protected override void Awake()
    {
        base.Awake();
        LoadData();
        InitializeData();
    }

    public void InitializeData()
    {
        coins = new Item(coinsData, 1000);
        hearts = new Item(heartsData, 5);
        //coins = SAVE.GetCoins();
        //hearts = SAVE.GetHearts();
    }
    public void CollectGift(string giftId, int quantity)
    {
        if(coins.itemData.Id == giftId)
        {
            DepositeCoins(quantity);
        }
        else if(hearts.itemData.Id == giftId)
        {
            
        }
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
        if(coins.quantity - coinsWithdraw >= 0)
        {   
            Observer.Instance.Broadcast(EventId.OnSpendCoins, coinsWithdraw);
            coins.quantity = coins.quantity - coinsWithdraw;
            Observer.Instance.Broadcast(EventId.OnUpdateCoins, coins.quantity);
            SAVE.SaveCoins(coins.quantity);
            return true;
        }
        return false;
    }
    public void DepositeCoins(int coinsDeposite)
    {
        coins.quantity += coinsDeposite;
        Observer.Instance.Broadcast(EventId.OnUpdateCoins, coins.quantity);
        SAVE.SaveCoins(coins.quantity);
    }
    public bool WithdrawHearts(int heartsWithdraw)
    {
        if (hearts.quantity - heartsWithdraw >= 0)
        {
            hearts.quantity = hearts.quantity - heartsWithdraw;
            Observer.Instance.Broadcast(EventId.OnUpdateHearts, hearts.quantity);
            SAVE.SaveHearts(hearts.quantity);
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
