using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragOnPlaneLogic : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private Plane dragPlane; 


    public Vector3 dragPlaneNormal = Vector3.forward; 

    private void Start()
    {
        dragPlane = new Plane(dragPlaneNormal, transform.position);
    }

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (dragPlane.Raycast(ray, out distance))
        {
            Vector3 newPosition = ray.GetPoint(distance) + offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        }
    }
}
