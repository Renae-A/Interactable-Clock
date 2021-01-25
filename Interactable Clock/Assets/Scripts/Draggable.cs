using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Transform draggable;

    // Reference: https://answers.unity.com/questions/1008678/i-cant-get-onmousedrag-to-work-with-gui-elements.html
    // Moves the UI Clock’s current position to the desired position on canvas within window of application.
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset = draggable.transform.position - transform.position;
        draggable.transform.position = eventData.position + offset;

        Cursor.lockState = CursorLockMode.Confined;
    }

    // Unlock mouse when the user stops dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
