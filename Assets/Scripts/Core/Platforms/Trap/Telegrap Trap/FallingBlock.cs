using UnityEngine;

public class FallingBlock : TrapBase
{
    [SerializeField] GameObject telegraphTrap;
    [SerializeField] float speed;
    bool canMove;
    bool isSpawned;
    private Vector2 lockPosition;
    [SerializeField] float distanceSpawnTele;
    [SerializeField] float distanceDisable;
    [SerializeField] float warningDuration;
    private void OnEnable()
    {
        canMove = true;
    }
    public void NotifyWhenTelegrapEnd()
    {
        canMove = true;
    }
    private void Update()
    {
        if (!isSpawned && transform.position.y - Camera.main.transform.position.y < distanceSpawnTele)
        {
            Spawn();
            lockPosition = transform.position;
            canMove = false;
            isSpawned = true;
        }
        if (!canMove)
        {

            if (isSpawned)
            {
                transform.position = lockPosition;
            }

            return;
        }
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (Camera.main.transform.position.y - transform.position.y > distanceDisable)
        {
            DestroySelf(false);
        }
    }
    void Spawn()
    {
        GameObject tmpObject = MyPoolManager.Instance.GetFromPool(telegraphTrap);
        tmpObject.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        TelegraphTrap tmpTele = tmpObject.GetComponent<TelegraphTrap>();
        if (tmpTele != null)
        {
            tmpTele.Initial(NotifyWhenTelegrapEnd, warningDuration);
        }

    }
    private void OnDisable()
    {
        canMove = false;
        isSpawned = false;
    }

}
