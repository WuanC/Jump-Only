using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpForceY;
    [SerializeField] float jumpForceX;
    Rigidbody2D rb;
    Vector3 startPosition;
    [SerializeField] float timeRespawn;
    bool isDead;
    bool isFristEnable = true;
    bool bootSpeed;

    [Header("Boots Speed")]
    private Collider2D playerCol;
    private Coroutine boosts;
    [SerializeField] float horizontalSpeed;


    public event Action<Vector2> OnPlayerStartFall;
    private void Awake()
    {
        playerCol = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        if (isFristEnable)
        {
            isFristEnable = false;
            return; 
        }
        Observer.Instance.Broadcast(EventId.OnPlayerRespawn, null);
        
    }
    private void Start()
    {
        startPosition = transform.position;
        Observer.Instance.Register(EventId.OnUserInput, Player_OnUserInput);
        Observer.Instance.Register(EventId.OnEnterJumpPad, Player_OnEnterJumpPad);
    }
    private void OnDisable()
    {
        if (boosts != null)
        {
            StopCoroutine(boosts);
            boosts = null;
        }
        bootSpeed = false;
        rb.velocity = Vector3.zero;
    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnUserInput, Player_OnUserInput);
        Observer.Instance.Unregister(EventId.OnEnterJumpPad, Player_OnEnterJumpPad);
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

    void Player_OnEnterJumpPad(object obj)
    {
        var tuple = ((float, Vector2))obj;
        if(boosts != null)
        {
            StopCoroutine(boosts);
            boosts = null;
        }
        boosts = StartCoroutine(OnEnterJumpPad(tuple.Item1));
    }
    IEnumerator OnEnterJumpPad(float duration)
    {
        rb.velocity = Vector3.zero;
        bootSpeed = true;
        yield return new WaitForSeconds(duration);
        bootSpeed = false;
        rb.velocity = Vector3.zero;
    }
    public void AddForceToPlayer(float xValue, float yValue)
    {
        rb.velocity = Vector3.zero;       
        rb.AddForce(new Vector2(xValue, yValue), ForceMode2D.Impulse);
    }


    public void Died()
    {
        if(isDead) return;
        isDead = true;
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        Observer.Instance.Broadcast(EventId.OnPlayerDied, transform.position);
        transform.position = startPosition;
        Invoke(nameof(RespawnPlayer), timeRespawn);
    }
    public void RespawnPlayer()
    {
        isDead = false;
        gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        if (bootSpeed)
        {
            rb.velocity = CheckInput() * horizontalSpeed;
        }
        if(rb.velocity.y < 0)
        {
            OnPlayerStartFall?.Invoke(rb.velocity.normalized);
        }
    }
    public Vector2 CheckInput()
    {
        float dirX = 0; ;
        float dirY = 0 ;
        if(Input.GetKey(KeyCode.LeftArrow)) {
            dirX = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dirX = 1;
        }
        else if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return Vector2.zero;
            }
            Vector2 inputPosition = Input.mousePosition;

            if (inputPosition.x < Screen.width / 2)
            {
                dirX = -1;
            }
            else
            {
                dirX = 1;
            }
        }
        if(Input.GetKey(KeyCode.UpArrow)) {
            dirY = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            dirY = -1;
        }
        return new Vector2(dirX, dirY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Observer.Instance.Broadcast(EventId.OnPlayerColliding, null);
        }
    }
}
