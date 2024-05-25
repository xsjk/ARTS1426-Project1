using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BrainMusicLogic : MonoBehaviour
{
    public AudioSource music;
    [Range(0, 1)]
    public float threshold;


    private float[] value = new float[1024];
    void Update()
    {
        music.GetOutputData(value, 0);
        float now = value.Max();
        // Debug.Log($"min={value.Min()} max={value.Max()}");

        if (-threshold < now && now < threshold)
            GetComponent<ParticleSystem>().Stop();
        else if(GetComponent<ParticleSystem>().isStopped)
            GetComponent<ParticleSystem>().Play();

    }

}
