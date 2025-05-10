using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator anim;
    private Tween rotateTween;
    private Quaternion startRotation;
    [SerializeField] Player player;
    const string ANIM_JUMP = "isJump";
    const string ANIM_FLY = "isFly";
    Coroutine flyCoroutine;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        startRotation = transform.rotation;
        player.OnPlayerStartFall += Player_OnPlayerStartFall;
        Observer.Instance.Register(EventId.OnPlayerJump, Player_OnPlayerStartJump);
        Observer.Instance.Register(EventId.OnPlayerDied, OnPlayerDied);
        Observer.Instance.Register(EventId.OnEnterJumpPad, Player_OnPlayerStartFly);
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
    private void Player_OnPlayerStartFly(object obj)
    {
        var tuple = ((float, Vector2))obj;
        Vector2 dir = tuple.Item2;
        float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        rotateTween = transform.DORotate(new Vector3(0, 0, angleZ - 90), 0.1f);

        if(flyCoroutine != null)
        {
            StopCoroutine(flyCoroutine);
            flyCoroutine = null;
        }
        flyCoroutine = StartCoroutine(Fly(tuple.Item1));
    }
    IEnumerator Fly(float duration)
    {
        anim.SetBool(ANIM_FLY, true);
        yield return new WaitForSeconds(duration);
        anim.SetBool(ANIM_FLY, false);
    }
    private void OnPlayerDied(object obj)
    {
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        transform.rotation = startRotation;
    }
    private void OnDisable()
    {
        if (flyCoroutine != null)
        {
            StopCoroutine(flyCoroutine);
            flyCoroutine = null;
        }
        anim.SetBool(ANIM_FLY, false);
    }
    private void OnDestroy()
    {
        player.OnPlayerStartFall -= Player_OnPlayerStartFall;
        Observer.Instance.Unregister(EventId.OnPlayerDied, OnPlayerDied);
        Observer.Instance.Unregister(EventId.OnPlayerJump, Player_OnPlayerStartJump);
        Observer.Instance.Unregister(EventId.OnEnterJumpPad, Player_OnPlayerStartFly);
        rotateTween?.Kill();
    }
}
