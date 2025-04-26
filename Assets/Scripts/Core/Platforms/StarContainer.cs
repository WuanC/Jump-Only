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
    public void AddStar()
    {
        currentStar++;
        if(currentStar == maxStar)
        {
            GameManager.Instance.PlayerWin();
        }
    }
}
