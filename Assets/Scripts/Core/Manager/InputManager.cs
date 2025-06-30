using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput(); 
        HandleKeyboardInput();
#else
        HandleTouchInput(); 
#endif
    }
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            Vector2 inputPosition = Input.mousePosition;

            if (inputPosition.x < Screen.width / 2)
            {
                Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Left);
            }
            else
            {
                Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Right);
            }
        }
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Right);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.E);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Q);
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began)
            {
                SendInput(touch.position);
            }
        }
        void SendInput(Vector2 inputPosition)
        {
            if (inputPosition.x < Screen.width / 2)
            {
                Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Left);
            }
            else
            {
                Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Right);
            }
        }
    }
}

public enum InputDirection
{
    Left,
    Right,
    E,
    Q,
}
