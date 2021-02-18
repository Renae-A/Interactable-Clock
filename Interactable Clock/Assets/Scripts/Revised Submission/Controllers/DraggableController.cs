using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableController : InteractableClockElement
{
    // Use of IDragHandler, IEndDragHandler reference: https://answers.unity.com/questions/1008678/i-cant-get-onmousedrag-to-work-with-gui-elements.html
    // Moves the UI Clock’s current position to the desired position on canvas within window of application.
    public void OnClockDrag(PointerEventData eventData, GameObject draggable, GameObject clock)
    {
        Vector2 offset = clock.transform.position - draggable.transform.position;
        clock.transform.position = eventData.position + offset;

        Cursor.lockState = CursorLockMode.Confined;
    }

    // Unlock mouse when the user stops dragging
    public void OnClockEndDrag(PointerEventData eventData)
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
