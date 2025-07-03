using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTransformModePlay : MonoBehaviour
{
    [SerializeField] EMoveMode moveMode;
    public void SetMoveMode(EMoveMode mode)
    {
        moveMode = mode;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player playerController))
        {
            Debug.LogWarning("Transform Mode: " + moveMode);
            Observer.Instance.Broadcast(EventId.OnChangePlayerMovement, moveMode);
        }
    }
}
