using Cinemachine;
using System.Collections;
using UnityEngine;

public class TrapDeadZoneCamera : TrapBase
{
    CinemachineVirtualCamera virtualCamera;
    SpriteRenderer sr;
    BoxCollider2D boxCol;
    public const float SIZE_PER_ONE_UNIT_CAMERA = 15f;
    public enum PositionByCamera
    {
        Up, Down,
    }
    public PositionByCamera position = PositionByCamera.Down;
    private void Awake()
    {
        virtualCamera = GetComponentInParent<CinemachineVirtualCamera>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        Observer.Instance.Register(EventId.OnPlayerDied, DeadZone_OnPlayerDied);
        Observer.Instance.Register(EventId.OnPlayerRespawn, DeadZone_OnPlayerSpawn);

        if(position == PositionByCamera.Down)
        {
            transform.localPosition = new Vector3(0, -virtualCamera.m_Lens.OrthographicSize + 0.3f, transform.localPosition.z);
        }
        else
        {
            Vector3 newLocalAngle = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + 180);
            transform.localEulerAngles = newLocalAngle;
            transform.localPosition = new Vector3(0, +virtualCamera.m_Lens.OrthographicSize - 0.3f, transform.localPosition.z);
        }

        float width = virtualCamera.m_Lens.OrthographicSize * 2 * virtualCamera.m_Lens.Aspect;
        sr.size = new Vector2(width, sr.size.y);
        boxCol.size = new Vector2(width, sr.size.y);
 

    }
    private void OnDestroy()
    {
        Observer.Instance.Unregister(EventId.OnPlayerDied, DeadZone_OnPlayerDied);
        Observer.Instance.Unregister(EventId.OnPlayerRespawn, DeadZone_OnPlayerSpawn);
    }
    void DeadZone_OnPlayerDied(object obj)
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
    IEnumerator EnableTrap()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
    void DeadZone_OnPlayerSpawn(object obj)
    {
        StartCoroutine(EnableTrap());
    }
}
