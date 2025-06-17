using System;
using System.Collections;
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

    Coroutine swapLane;



    private PlayerBoost playerBoost;
    private PlayerVisual visual;
    [SerializeField] LayerMask trapLayer;
    [SerializeField] float durationIgnore;

    public event Action<Vector2> OnPlayerStartFall;

    [Header("Test new input")]
    public bool isNewInput = false;
    [SerializeField] Vector2[] line = new Vector2[3];
    [SerializeField] int currentLine = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerBoost = GetComponent<PlayerBoost>();
    }
    private void OnEnable()
    {
        if (isFristEnable)
        {
            isFristEnable = false;
            return;
        }
        currentLine = 1;
        Observer.Instance.Broadcast(EventId.OnPlayerRespawn, GameManager.Instance.gameMode);

    }
    private void Start()
    {

        Observer.Instance.Register(EventId.OnUserInput, Player_OnUserInput);
        if (isNewInput) transform.position = line[1];
        startPosition = transform.position;
    }
    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUserInput, Player_OnUserInput);

        int trapLayerIndex = Mathf.RoundToInt(Mathf.Log(trapLayer.value, 2));
        Physics2D.IgnoreLayerCollision(gameObject.layer, trapLayerIndex, false);
    }
    public void SetVisual(PlayerVisual visual)
    {
        this.visual = visual;
    }
    public void Player_OnUserInput(object obj)
    {
        if (isDead) return;
        InputDirection dir = (InputDirection)obj;
        if (!isNewInput)
        {
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
        else
        {
            int height = 3;
            if (dir == InputDirection.Left && currentLine > 0)
            {
                currentLine = currentLine - 1;
                
                if(swapLane != null)
                {
                    StopCoroutine(swapLane);
                }
                if(gameObject.activeSelf)
                swapLane = StartCoroutine(SwapLane(0.25f, transform.position, line[currentLine], height));
            }
            else if (dir == InputDirection.Right && currentLine < line.Length - 1)
            {
                currentLine = currentLine + 1;
                if (swapLane != null)
                {
                    StopCoroutine(swapLane);

                }
                if(gameObject.activeSelf)
                swapLane = StartCoroutine(SwapLane(0.25f, transform.position, line[currentLine], height));
            }

        }

    }
    IEnumerator SwapLane(float timeToSwapLane, Vector3 pos1, Vector3 pos2, float height)
    {

        float t = 0;
        while (t < timeToSwapLane)
        {
            Vector3 targetPos = BezierCurve(pos1,new Vector3((pos1.x + pos2.x) / 2, pos1.y + height, 0), pos2, t / timeToSwapLane);
            Vector3 dir = targetPos - transform.position;
            transform.position = targetPos;
            t += Time.deltaTime;
            Observer.Instance.Broadcast(EventId.OnPlayerJump, new Vector2(dir.x, dir.y).normalized);



            yield return null;
        }
        visual.ResetVisual();
    }
    protected Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
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
        if (GameManager.Instance.gameMode == EGameMode.Endless)
        {
            GameManager.Instance.RespawnEndless(this);
        }
        else if (GameManager.Instance.gameMode == EGameMode.Adventure)
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
