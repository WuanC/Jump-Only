using System;
using UnityEngine;

public abstract class QuestBase : MonoBehaviour
{
    public QuestData questData;
    public Action<QuestType, int> OnTrackingQuest; //id
    public int CurrentAmount {  get; protected set; }
    public Gift gift;

    public virtual void Start()
    {
        Initial();
    }
    public abstract void Initial();
    public virtual void OnCompletedQuest()
    {
        OnTrackingQuest = null;
    }
}
