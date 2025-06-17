using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandleConflictScrollbar :  MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] ScrollRect scrollRectParent;
    bool parentProcess;
    Vector2 startPosDrag;


    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosDrag = eventData.position;
        parentProcess = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!parentProcess)
        {
            Vector2 dragDelta = eventData.position - startPosDrag;
            if(dragDelta != Vector2.zero)
            {
                if (Mathf.Abs(dragDelta.y) < Mathf.Abs(dragDelta.x))
                {
                    parentProcess = true;
                    ExecuteEvents.ExecuteHierarchy(scrollRectParent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                    ExecuteEvents.ExecuteHierarchy(scrollRectParent.gameObject, eventData, ExecuteEvents.dragHandler);
                }
                var delta = eventData.delta;
                delta.x = 0;
                eventData.delta = delta;
            }
  

        }
        else if(parentProcess ) 
        {
            var delta = eventData.delta;
            delta.y = 0;
            eventData.delta = delta;
            ExecuteEvents.ExecuteHierarchy(scrollRectParent.gameObject, eventData, ExecuteEvents.dragHandler);         
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentProcess)
        {
            parentProcess = false;
            ExecuteEvents.ExecuteHierarchy(scrollRectParent.gameObject, eventData, ExecuteEvents.endDragHandler);
        }
    }

}
