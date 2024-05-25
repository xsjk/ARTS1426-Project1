using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting.FullSerializer.Internal;
using System;


public class RhythmVisualizatorPro2 : MonoBehaviour
{

    #region Variables

    public GameObject linePrefab;
    public Transform linesTransform;

    [Header("AUDIO SETTINGS")]
    public bool listenAllSounds;
    public AudioSource audioSource;

    [Space(5)]

    [Header("SOUND BARS")]


    [Range(32, 256)]
    public int quantity = 64;

    int n;


    [Header("VISUALIZATION")]

    [Range(0, 10)]
    public int numberOfCircles = 1;
    public float minCircleRadius = 0.1f;
    public float maxCircleRadius = 0.4f;


    [Range(0f, 10f)]
    public float extraScaleVelocity = 50f;

    [Header("LEVELS")]

    [Range(1, 15)]
    public int smoothVelocity = 3;

    public enum Channels { n512, n1024, n2048, n4096, n8192 };

    public Channels channels = Channels.n4096;
    public FFTWindow method = FFTWindow.Blackman;
    int channelValue;

    [Header("FREQUENCY SETTINGS")]

    public float freqThreshold = 0.002f;

    public int freqHorizontalScale = 1;
    public int freqOffset = 0;


    [Header("APPEARANCE")]

    [HideInInspector]
    public GradientController gradient;

    #endregion

    #region Start


    private LineRenderer[] lineRenderers;
    private float[] radius;


    void Awake() {
        
        // Check the prefabs
        if (linePrefab == null) {
            Debug.LogError("Please assign Sound Bar Prefabs to the script");
            enabled = false;
            return;
        }

        if (linesTransform == null) {
            Debug.LogError("Please assign the Lines Transform to the script");
            enabled = false;
            return;
        }

        if (audioSource == null)
            listenAllSounds = true;

        // Initialize n
        if (quantity % 4 != 0)
            quantity += quantity % 4;
        if (quantity < 32)
            quantity = 32;
        else if (quantity > 256)
            quantity = 256;
        n = quantity;
        
        // Initialize gradient (Color Generator)
        gradient = transform.parent.GetComponent<GradientController>();

        // Initialize rhythm lines
        lineRenderers = new LineRenderer[numberOfCircles];
        for (int i = 0; i < numberOfCircles; i++) {
            var l = Instantiate(linePrefab, linesTransform);
            l.name = $"Rhythm Line {i+1}";
            lineRenderers[i] = l.GetComponent<LineRenderer>();
            lineRenderers[i].positionCount = n;
        }
        radius = new float[n];

        // Initialize channelValue
        switch (channels)
        {
            case Channels.n512:
                channelValue = 512;
                break;
            case Channels.n1024:
                channelValue = 1024;
                break;
            case Channels.n2048:
                channelValue = 2048;
                break;
            case Channels.n4096:
                channelValue = 4096;
                break;
            case Channels.n8192:
                channelValue = 8192;
                break;
        }
    }

    void Start()
    {

    }

    #endregion

    #region BaseScript

    void Update()
    {

        // Get Spectrum Data from Both Channels of audio
        float[] spectrumLeftData = new float[channelValue];
        float[] spectrumRightData = new float[channelValue];

        if (listenAllSounds)
        {
            AudioListener.GetSpectrumData(spectrumLeftData, 0, method);
            AudioListener.GetSpectrumData(spectrumRightData, 1, method);
        }
        else
        {
            audioSource.GetSpectrumData(spectrumLeftData, 0, method);
            audioSource.GetSpectrumData(spectrumRightData, 1, method);
        }

        // Scale SoundBars Normally
        for (int i = 0; i < (n / 2); i++)
        {
            int idx = i * freqHorizontalScale + freqOffset;
            UpdateScale(ref radius[i], spectrumLeftData[idx]);
            UpdateScale(ref radius[n - i - 1], spectrumRightData[idx]);
        }


        void UpdateScale(ref float scale, float spectrumValue) {
            float newScale = Mathf.Lerp(scale, Mathf.Log(spectrumValue / freqThreshold + 1), Time.deltaTime * extraScaleVelocity);
            if (newScale > scale)
                scale = newScale;
            else
                scale = Mathf.Lerp(scale, 1, Time.deltaTime * smoothVelocity);
        }
    }


    void LateUpdate() {

        if (numberOfCircles == 1) {
            UpdateLine(0, maxCircleRadius, 0);
        } 
        else if (numberOfCircles > 1) {
            for (int j = 0; j < numberOfCircles; j++)
            {
                float t = (float)j / (numberOfCircles - 1);
                float k = Mathf.LerpUnclamped(minCircleRadius, maxCircleRadius, t * t);
                float d = Mathf.LerpUnclamped(-0.5f, 0.5f, Mathf.Pow(t, 0.25f) * 0.2f);
                UpdateLine(j, k, d);
            }
        }
    }

    void UpdateLine(int j, float k, float d) {
        var l = lineRenderers[j];
        l.positionCount = n;
        for (int i = 0; i < n; i++)
        {
            float angle = i * Mathf.PI * 2 / n;
            l.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), d) * (k * radius[i]));
        }

    }

    #endregion

}