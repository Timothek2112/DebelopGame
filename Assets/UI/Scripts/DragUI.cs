using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUI : MonoBehaviour
{
    [SerializeField] GameObject draggingObject;
    bool dragging = false;
    Vector2 dragOffset = Vector2.zero;

    private void Update()
    {
        if (dragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            draggingObject.transform.position = mousePos - dragOffset;
        }
    }

    private void OnMouseDown()
    {
        dragging = true;
        dragOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - draggingObject.transform.position;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
