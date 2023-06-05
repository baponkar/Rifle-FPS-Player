using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightControl : MonoBehaviour
{
    Light spotLight;
    public bool lightOn;
    AudioSource audioSource;

    void Start()
    {
        spotLight = GetComponent<Light>();
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            lightOn = !lightOn;
        LightOnOff(lightOn);
        
    }

    void LightOnOff(bool on)
    {
        spotLight.enabled = on;
    }
}
