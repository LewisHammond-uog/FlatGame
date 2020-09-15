using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailDoorEvent : EventObject
{
    [Header("Door Light")] 
    [SerializeField] private Light jailDoorStatusLight;
    [SerializeField] private Color startCol;
    [SerializeField] private Color endCol;

    [Header("Door Material")] 
    [SerializeField] private Material startMaterial;
    [SerializeField] private Material endMaterial;
        
    [Header("Door Movement")]
    //Final Position of the jail door
    [SerializeField] private Vector3 stopPosition;

    //Jail Move Speed
    [SerializeField] private float moveSpeed;

    [SerializeField] private AudioClip audio;

    [SerializeField] private AudioSource soruce;

    //If we are currently moving
    private bool isActivated = false;

    private void Start()
    {
        //Set Colour of light
        if (jailDoorStatusLight)
        {
            jailDoorStatusLight.color = startCol;
        }

        //Material
        GetComponent<MeshRenderer>().material = startMaterial;
    }

    private void Update()
    {
        if (isActivated)
        {
            //Check if we should stop
            if (transform.position == stopPosition)
            {
                isActivated = false;
            }

            //Play Door open animation
            transform.position = Vector3.Lerp(transform.position, stopPosition, moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Trigger Event to open jail door
    /// </summary>
    public override void TriggerEvent()
    {
        isActivated = true;

        //Change Light Colour
        if (jailDoorStatusLight)
        {
            jailDoorStatusLight.color = endCol;
        }

        //Switch Material
        GetComponent<MeshRenderer>().material = endMaterial;

        //Play Audio
        if (audio && soruce)
        {
            soruce.PlayOneShot(audio);
        }
    }
}