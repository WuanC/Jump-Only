using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpForceY;
    [SerializeField] float jumpForceX;
    Rigidbody2D rb;
    Vector3 startPosition;
    [SerializeField] float timeRespawn;
    bool isDead;
    bool isFristEnable = true;


    private PlayerBoost playerBoost;
    private PlayerVisual visual;
    [SerializeField] LayerMask trapLayer;
    [SerializeField] float durationIgnore;

    public event Action<Vector2> OnPlayerStartFall;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerBoost = GetComponent<PlayerBoost>();
        visual = GetComponentInChildren<PlayerVisual>();
    }
    private void OnEnable()
    {
        if (isFristEnable)
        {
            isFristEnable = false;
            return;
        }
        Observer.Instance.Broadcast(EventId.OnPlayerRespawn, GameManager.Instance.gameMode);

    }
    private void Start()
    {
        startPosition = transform.position;
        Observer.Instance.Register(EventId.OnUserInput, Player_OnUserInput);

    }
    private void OnDisable()
    {

        rb.velocity = Vector3.zero;
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUserInput, Player_OnUserInput);

    }

    public void Player_OnUserInput(object obj)
    {
        InputDirection dir = (InputDirection)obj;
        if (dir == InputDirection.Left)
        {
            Observer.Instance.Broadcast(EventId.OnPlayerJump, new Vector2(-jumpForceX, jumpForceY).normalized);
            AddForceToPlayer(-jumpForceX, jumpForceY);
        }
        else if (dir == InputDirection.Right)
        {
            Observer.Instance.Broadcast(EventId.OnPlayerJump, new Vector2(jumpForceX, jumpForceY).normalized);
            AddForceToPlayer(jumpForceX, jumpForceY);
        }

    }




    public void AddForceToPlayer(float xValue, float yValue)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(xValue, yValue), ForceMode2D.Impulse);
    }


    public void Died()
    {
        if (isDead) return;
        if (playerBoost.IsInvicibility)
        {
            playerBoost.NotifyEventOnPlayerDied();
            return;
        }
        isDead = true;
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        Observer.Instance.Broadcast(EventId.OnPlayerDied, transform.position);
        if(GameManager.Instance.gameMode == EGameMode.Endless)
        {
            GameManager.Instance.RespawnEndless(this);
        }
        else if(GameManager.Instance.gameMode == EGameMode.Adventure)
        {
            Invoke(nameof(RespawnPlayer), timeRespawn);
        }


    }
    public void RespawnPlayer()
    {
        transform.position = startPosition;
        isDead = false;
        gameObject.SetActive(true);
    }
    public void RespawnEndless()
    {
        isDead = false;
        gameObject.SetActive(true);
        StartCoroutine(IgnoreTrap(durationIgnore));
    }
    public IEnumerator IgnoreTrap(float duration)
    {
        int trapLayerIndex = Mathf.RoundToInt(Mathf.Log(trapLayer.value, 2));
        Physics2D.IgnoreLayerCollision(gameObject.layer, trapLayerIndex, true);
        visual.Immortal(true);
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreLayerCollision(gameObject.layer, trapLayerIndex, false);
        visual.Immortal(false);
    }
    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            OnPlayerStartFall?.Invoke(rb.velocity.normalized);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Observer.Instance.Broadcast(EventId.OnPlayerColliding, null);
        }
    }
   
}
