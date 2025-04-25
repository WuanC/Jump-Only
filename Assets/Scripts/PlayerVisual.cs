using DG.Tweening;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator anim;
    private Tween rotateTween;
    private Quaternion startRotation;
    [SerializeField] Player player;
    const string ANIM_JUMP = "isJump";
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        startRotation = transform.rotation;
        player.OnPlayerStartJump += Player_OnPlayerStartJump;
        player.OnPlayerStartFall += Player_OnPlayerStartFall;
        Observer.Instance.Register(EventId.OnPlayerDied, OnPlayerDied);
    }

    private void Player_OnPlayerStartFall(Vector2 obj)
    {
        float angleZ = Mathf.Atan2(obj.y, obj.x) * Mathf.Rad2Deg;
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        rotateTween = transform.DORotate(new Vector3(0, 0, angleZ + 90), 0.5f);
    }

    private void Player_OnPlayerStartJump(Vector2 obj)
    {
        anim.SetTrigger(ANIM_JUMP);
        float angleZ = Mathf.Atan2(obj.y, obj.x) * Mathf.Rad2Deg;
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        rotateTween = transform.DORotate(new Vector3(0, 0, angleZ - 90), 0.1f);
    }
    private void OnPlayerDied(object obj)
    {
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        transform.rotation = startRotation;
    }
    private void OnDestroy()
    {
        player.OnPlayerStartJump -= Player_OnPlayerStartJump;
        player.OnPlayerStartFall -= Player_OnPlayerStartFall;
        Observer.Instance.Unregister(EventId.OnPlayerDied, OnPlayerDied);
        rotateTween?.Kill();
    }
}
