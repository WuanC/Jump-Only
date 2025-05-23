using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
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
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Observer.Instance.Broadcast(EventId.OnUserInput, InputDirection.Left);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
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
}
