using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] float doubleTapThreshold = 0.3f;
    float lastTapTime = 0f;
    Vector2 startTouchPosition;
    Vector2 endTouchPosition;
    [SerializeField] float minSwipeDistance = 50f;
    float maxTapDistance = 3f;
    Vector2 lastTapPosition = Vector2.zero;
    bool ignoreNextTap = false;
    float tapDelay = 0.05f;

    Coroutine myCou;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        //HandleMouseInput(); 
        HandleKeyboardInput();
        if (HandleSwipeUp()) return;
        HandleTouchInput();
#else

        if(HandleSwipeUp()) return;

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
    Vector2 startPos;
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                myCou = StartCoroutine(SendInput(touch.position));
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (Vector2.Distance(startPos, touch.position) > maxTapDistance)
                {
                    if (myCou != null)
                    {
                        StopCoroutine(myCou);
                        myCou = null;
                    }
                }
            }
        }

    }
    IEnumerator SendInput(Vector2 inputPosition)
    {
        yield return new WaitForSeconds(tapDelay);
        if (inputPosition.x < Screen.width / 2)
        {
            Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Left);
        }
        else
        {
            Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Right);
        }
    }

    bool HandleSwipeUp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    float swipeDistanceY = endTouchPosition.y - startTouchPosition.y;

                    if (Mathf.Abs(swipeDistanceY) > minSwipeDistance && swipeDistanceY > 0)
                    {
                        Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Q);
                        return true;
                    }
                    else if(Mathf.Abs(swipeDistanceY) > minSwipeDistance && swipeDistanceY < 0)
                    {
                        Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.E);
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}

public enum InputDirection
{
    Left,
    Right,
    E,
    Q,
}
