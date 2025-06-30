using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagBoost : UsageBoost
{
    public override void Excute()
    {
        base.Excute();
        Observer.Instance.Broadcast(EventId.OnChangeMap, EMoveMode.Zigzag);
        Use();
    }


    public override void Deactive()
    {
        base.Deactive();
    }
}
