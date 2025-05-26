using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestBase : MonoBehaviour
{
    public QuestData questData;
    public Action<QuestType, int> OnTrackingQuest; //id
    public int CurrentAmount;
    public virtual void Start()
    {
        Initial();
    }
    public abstract void Initial();
    public abstract void OnCompletedQuest();
}
