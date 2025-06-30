using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTransformModePlay : MonoBehaviour
{
    [SerializeField] EMoveMode moveMode;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player playerController))
        {
            Observer.Instance.Broadcast(EventId.OnChangePlayerMovement, moveMode);
        }
    }
}
