using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableView : InteractableClockElement, IDragHandler, IEndDragHandler
{
    public GameObject clock;

    // Notify controller of drag event
    public void OnDrag(PointerEventData eventData)
    {
        app.Notify(InteractableClockNotification.OnDrag, gameObject, clock, eventData);
    }

    // Notify controller of end drag event
    public void OnEndDrag(PointerEventData eventData)
    {
        app.Notify(InteractableClockNotification.OnRelease, eventData);
    }
}
