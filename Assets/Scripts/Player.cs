using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpForceY;
    [SerializeField] float jumpForceX;
    Rigidbody2D rb;
    Vector3 startPosition;
    [SerializeField] float timeRespawn;


    public event Action<Vector2> OnPlayerStartFall;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Died();
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddForceToPlayer(-jumpForceX, jumpForceY);
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddForceToPlayer(jumpForceX, jumpForceY);
        }
    }
    public void AddForceToPlayer(float xValue, float yValue)
    {
        rb.velocity = Vector3.zero;
        Observer.Instance.Broadcast(EventId.OnPlayerJump, new Vector2(xValue, yValue).normalized);
        rb.AddForce(new Vector2(xValue, yValue), ForceMode2D.Impulse);
    }


    public void Died()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        Observer.Instance.Broadcast(EventId.OnPlayerDied, transform.position);
        Invoke(nameof(RespawnPlayer), timeRespawn);
    }
    public void RespawnPlayer()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        if(rb.velocity.y < 0)
        {
            OnPlayerStartFall?.Invoke(rb.velocity.normalized);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("Player");
            Observer.Instance.Broadcast(EventId.OnPlayerColliding, null);
        }
    }
}
