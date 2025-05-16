using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private Tween rotateTween;
    private Tween fadeTween;
    private Quaternion startRotation;
    [SerializeField] Player player;
    const string ANIM_JUMP = "isJump";
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        startRotation = transform.rotation;
        player.OnPlayerStartFall += Player_OnPlayerStartFall;
        Observer.Instance.Register(EventId.OnPlayerJump, Player_OnPlayerStartJump);
        Observer.Instance.Register(EventId.OnPlayerDied, OnPlayerDied);
    }

    private void Player_OnPlayerStartFall(Vector2 obj)
    {
        float angleZ = Mathf.Atan2(obj.y, obj.x) * Mathf.Rad2Deg;
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        rotateTween = transform.DORotate(new Vector3(0, 0, angleZ + 90), 0.5f);
    }

    private void Player_OnPlayerStartJump(object obj)
    {
        Vector2 dir = (Vector2)obj;
        anim.SetTrigger(ANIM_JUMP);
        float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
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
    public void Immortal(bool isImortal)
    {
        if (isImortal)
        {
            sr.DOFade(0.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            fadeTween?.Kill();
            Color color = sr.color;
            color.a = 1f;
            sr.color = color;
            sr.DOKill();
        }
    }
    private void OnDestroy()
    {
        player.OnPlayerStartFall -= Player_OnPlayerStartFall;
        Observer.Instance.Unregister(EventId.OnPlayerDied, OnPlayerDied);
        Observer.Instance.Unregister(EventId.OnPlayerJump, Player_OnPlayerStartJump);

        rotateTween?.Kill();
        fadeTween?.Kill();
    }
}
