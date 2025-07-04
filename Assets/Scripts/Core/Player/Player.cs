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

    [Header("Input")]
    public EMoveMode moveMode;
    [SerializeField] Vector2[] line = new Vector2[3];
    [SerializeField] int currentLine = 1;

    public bool zigzagMode = false;
    [SerializeField] float speedZigzag;
    enum ZigzagMove
    {
        Up, Left, Right
    }
    ZigzagMove zigzagMove = ZigzagMove.Up;
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
        Observer.Instance.Register(EventId.OnChangePlayerMovement, Player_OnChangePlayerMoveMode);
        if (moveMode == EMoveMode.ThreeLine) transform.position = line[1];
        startPosition = transform.position;
    }
    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUserInput, Player_OnUserInput);
        Observer.Instance.Unregister(EventId.OnChangePlayerMovement, Player_OnChangePlayerMoveMode);
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
        if (moveMode == EMoveMode.Default)
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
        else if(moveMode ==  EMoveMode.ThreeLine)
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
        else if(moveMode == EMoveMode.Zigzag)
        {
            if(dir == InputDirection.Left)
            {
                zigzagMove = ZigzagMove.Left;

            }
            else if( dir == InputDirection.Right)
            {
                zigzagMove = ZigzagMove.Right;
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
        transform.position = startPosition;
        gameObject.SetActive(true);
        StartCoroutine(IgnoreTrap(durationIgnore));

        isDead = false;


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
        if(moveMode == EMoveMode.Zigzag)
        {
            if (zigzagMove == ZigzagMove.Right)
            {
                visual.Rotate(45);
                rb.velocity = new Vector2(speedZigzag, 0);
            }
            else if(zigzagMove == ZigzagMove.Left)
            {
                visual.Rotate(135);
                rb.velocity = new Vector2(-speedZigzag, 0);
            }
        }
    }

    void Player_OnChangePlayerMoveMode(object obj)
    {
        EMoveMode moveMode = (EMoveMode)obj;
        this.moveMode = moveMode;
        rb.velocity = Vector2.zero;
        if(moveMode == EMoveMode.Zigzag)
        {
            zigzagMove = ZigzagMove.Up;
        }
        else if(moveMode == EMoveMode.ThreeLine)
        {
            currentLine = 1;
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
