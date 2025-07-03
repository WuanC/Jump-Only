using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoost : UsageBoost//, IActivationBoost
{
    [SerializeField] LayerMask trapLayer;
    [SerializeField] float distanceDestroy;
    public bool HasBoost(string key)
    {
        return Inventory.Instance.itemDics.ContainsKey(key);
    }
    public override void Excute()
    {
        base.Excute();
        //if (HasBoost(boostData.Id))
        //{
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, distanceDestroy, trapLayer);
            foreach (var obstacle in hit)
            { 
                if (obstacle.TryGetComponent<TrapBase>(out TrapBase trap))
                {
                    if(!trap.cantDestroy)
                    trap.DestroySelf();
                }
            }
            //Item item = new Item(boostData, -1);
            //Inventory.Instance.UseItem(item);
            Use();
        //}
        //else
        //{
        //    Deactive();
        //}

    }
    public override void Deactive()
    {
        base.Deactive();

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, distanceDestroy);
    }


}

