using UnityEngine;
using UnityEngine.VFX;

public class FallingBlock : TrapBase
{
    [SerializeField] GameObject telegraphTrap;
    [SerializeField] float speed;
    bool canMove;
    bool isSpawned;
    [SerializeField] float distanceSpawn;
    [SerializeField] float distanceDisable;
    [SerializeField] float warningDuration; 
    public void NotifyWhenTelegrapEnd()
    {
        canMove = true;
    }
    private void Update()
    {
        if(!isSpawned && transform.position.y - Camera.main.transform.position.y > distanceSpawn)
        {
            Spawn();
            isSpawned = true;
        }
        if (!canMove) return;
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (Camera.main.transform.position.y - transform.position.y > distanceDisable)
        {
            gameObject.SetActive(false);
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
