using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPosition : MonoBehaviour
{
    public float speed = 1.0f;
    
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * speed);
    }
}
