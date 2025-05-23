using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarContainer : MonoBehaviour
{
    [SerializeField] int maxStar;
    int currentStar;
    private void Start()
    {
        currentStar = 0;
    }
    public void MinusStar()
    {
        if (currentStar == 0) return;
        currentStar--;
    }
    public void AddStar()
    {
        currentStar++;
        if(currentStar == maxStar)
        {
            Observer.Instance.Broadcast(EventId.OnPlayerWin, null);
            GameManager.Instance.PlayerWin();
        }
    }
}
