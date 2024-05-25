using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{

    public float width = 0.01f;
    public float r = 0.01f;
    public int n = 100;

    private LineRenderer lineRenderer;

    private Vector3[] points;
    private float[] radius;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.loop = true;

        points = new Vector3[n];
        radius = new float[n];
        
    }


    void Update()
    {
        lineRenderer.startWidth = width;
        
        if (n != points.Length) {
            points = new Vector3[n];
            radius = new float[n];
        }
        
        for (int i = 0; i < n; i++)
        {
            radius[i] = r;
            
            float angle = i * Mathf.PI * 2 / n;
            points[i] = radius[i] * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        }

        UpdatePositions();
    }

    
    private void UpdatePositions() {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}
