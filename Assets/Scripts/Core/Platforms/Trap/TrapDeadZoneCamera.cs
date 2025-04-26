using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDeadZoneCamera : TrapBase
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    public const float SIZE_PER_ONE_UNIT_CAMERA = 6.4f;
    private void Awake()
    {
        virtualCamera = GetComponentInParent<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        transform.localPosition = new Vector3(0, -virtualCamera.m_Lens.OrthographicSize, transform.localPosition.z);
        float width = virtualCamera.m_Lens.OrthographicSize * 2 * virtualCamera.m_Lens.Aspect;
        transform.localScale = new Vector3(width * SIZE_PER_ONE_UNIT_CAMERA, transform.localScale.y, transform.localScale.z);

    }
}
