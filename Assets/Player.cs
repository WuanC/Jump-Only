using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpForceY;
    [SerializeField] float jumpForceX;
    Rigidbody2D rb;
    Vector3 startPosition;
    [SerializeField] float timeRespawn;
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
        rb.AddForce(new Vector2(xValue, yValue), ForceMode2D.Impulse);
    }


    public void Died()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        Invoke(nameof(RespawnPlayer), timeRespawn);
    }
    public void RespawnPlayer()
    {
        transform.position = startPosition;
    }
}
