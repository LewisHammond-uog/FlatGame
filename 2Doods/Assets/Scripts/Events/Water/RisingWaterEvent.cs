using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWaterEvent : EventObject
{
    //If we should start as soon as the object is enabled
    [SerializeField] private bool startOnAwake = false;

    //Vector for end position of rising water
    [Header("Water Plane")] 
    [SerializeField] private GameObject waterPlane;
    [SerializeField] private Vector3 waterEndPos;
    [SerializeField] private float waterMoveSpeed;
    [SerializeField] private float waterMoveSmoothing;
    private Vector3 currentVelocity;

    //Water effect volume
    [Header("Water Post Processing Effect")]
    [SerializeField] private GameObject waterPPVVolume;
    //The floor that the water should go down to
    private GameObject waterFloor;

    //Enum for the state of the water
    private enum WaterState
    {
        NotStarted, //Water rising has not been triggered
        Rising,     //Water is currently rising
        Finished    //Water has risen to it's end pos
    }
    private WaterState currentWaterState = WaterState.NotStarted;

    // Start is called before the first frame update
    void Start()
    {
        if (startOnAwake)
        {
            TriggerEvent();
        }

        //Get the floor object so we know where the bottom of our PPV should be
        if (Physics.Raycast(new Ray(waterPlane.transform.position, -waterPlane.transform.up), out RaycastHit rayHit))
        {
            waterFloor = rayHit.collider.gameObject;
        }

        //Update the PPV Volume so it matches up
        if (waterPPVVolume)
        {
            UpdatePPVVolumeSize();
        }
    }

    /// <summary>
    /// Trigger the start of the water rising
    /// </summary>
    public override void TriggerEvent()
    {
        //Move the state to water rising if it hasn't
        //started yet
        if (currentWaterState == WaterState.NotStarted)
        {
            //Start the water rising
            currentWaterState = WaterState.Rising;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //Check if water should be rising, then if it is move the water up
        if (currentWaterState == WaterState.Rising)
        {
            //Smoothly move towards our end position
            if (waterPlane)
            {
                waterPlane.transform.localPosition = Vector3.SmoothDamp(transform.localPosition, waterEndPos, ref currentVelocity,
                    waterMoveSmoothing, waterMoveSpeed, Time.deltaTime);


                //Update the water post processing volume
                if (waterPPVVolume)
                {
                    UpdatePPVVolumeSize();
                }

                //Check if we have reached our end position. If we have
                //then stop the water from rising
                if (transform.position == waterEndPos)
                {
                    currentWaterState = WaterState.Finished;
                }
            }
        }
    }

    /// <summary>
    /// Update the Post processing volumes size so that fits between the floor
    /// and the water plane
    /// </summary>
    private void UpdatePPVVolumeSize()
    {
        if (waterFloor && waterPlane)
        {
            //Our max Y should the the plane Y minus half it's scale
            //Our min Y should the floors Y minus half it's scale
            float waterPPVMaxY = waterPlane.transform.position.y;
            float waterPPVMinY = waterFloor.transform.position.y - waterFloor.transform.localScale.y * 0.5f;

            //Position should be the average of our Min and Max Y Positions
            waterPPVVolume.transform.position = new Vector3(waterPPVVolume.transform.position.x,
                (waterPPVMinY + waterPPVMaxY) * 0.5f, waterPPVVolume.transform.position.z);

            //Scale should be the difference between the max and min y
            waterPPVVolume.transform.localScale = new Vector3(waterPPVVolume.transform.localScale.x,
                (waterPPVMaxY - waterPPVMinY), waterPPVVolume.transform.localScale.z);

        }
    }
}
