using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaryingLight : MonoBehaviour
{
    [Header("Light")] 
    [SerializeField] private Light affectedLight;


    [Header("Light Change Settings")]
    //Update freqnency (time in seconds to wait between update)
    [SerializeField] private float updateFreq = 1f;
    
    //Min and max light intensity values
    [SerializeField] private float minLight, maxLight = 1f;

    //Smoothness for the light update
    [SerializeField] private float lightChangeSpeed; 
    
    //Current Target Light Intensity
    private float targetLightIntensity;

    private float timeSinceLightUpdate;

    private void Start()
    {
        //Initlise Time since last Update
        timeSinceLightUpdate = updateFreq;
    }

    private void Update()
    {

        //Add to update time
        timeSinceLightUpdate += Time.deltaTime;

        //Lerp towards target light value
        affectedLight.intensity =
            Mathf.Lerp(affectedLight.intensity, targetLightIntensity, lightChangeSpeed * Time.deltaTime);

        //If we should update a new target intensity then get one
        if (timeSinceLightUpdate >= updateFreq)
        {
            //Choose a new light intensity
            targetLightIntensity = Random.Range(minLight, maxLight);

            //Reset Timer
            timeSinceLightUpdate = 0f;
        }


    }

}
