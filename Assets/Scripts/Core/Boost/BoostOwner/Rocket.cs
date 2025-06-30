using UnityEngine;

public class Rocket : MonoBehaviour
{
    float speed;
    Vector3 direction;
    public void Init(float speed, Vector2 direction)
    {
        this.speed = speed;
        this.direction = direction;
        Invoke(nameof(DestroySelf), 5f);
    }
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<TrapBase>(out TrapBase trapBase))
        {
            Debug.Log("collid");
            trapBase.DestroySelf();
            DestroySelf();
        }

    }

}
