using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragLogic : MonoBehaviour
{
    private Vector3 offset;

    private float zCoord;

    private void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

}

