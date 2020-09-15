using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction3D : PlayerInteraction
{
    //Camera Component Attached to the player object
    [SerializeField]
    private Camera playerCamera;

    //Maximum distance that we can interact with an object from
    [SerializeField]
    private float interactDistance = 10f;

    //Property of the object that we are currently looking at
    public GameObject LookingAtObject { get { return GetLookingAtObject(); } }

    //Raycast Start Distance
    private const float rayStartDist = 0.3f;

    private void Start()
    {
        if (!playerCamera)
        {
            Debug.LogWarning("PlayerInteraction3D does not have camera component - objects will not be able to be picked up");
        }
    }

    /// <summary>
    /// Get the object that the player is looking at
    /// </summary>
    private GameObject GetLookingAtObject()
    {
        // Calculate the mouse postion in and offset position
        Vector3 mousePositionWithZOffset = Input.mousePosition;
        mousePositionWithZOffset.z = playerCamera.farClipPlane;

        //Where the camera is viewing from
        Vector3 cameraPosition = playerCamera.transform.position + (transform.forward * rayStartDist);

        //Get the postion 10 units away from the mouse click down the camera frustrum
        Vector3 mousePostionInWorldSpace = Camera.main.ScreenToWorldPoint(mousePositionWithZOffset);

        //The Vector Diffrence fron the camera, to the click postion down frustrum
        //Gets thoe direction of the mouse clock goind down the camera frustrum
        Vector3 directionFromCameraToMouse = mousePostionInWorldSpace - cameraPosition;

        //Checks a ray cast if there is an object we are looking at
        if (Physics.Raycast(new Ray(cameraPosition, directionFromCameraToMouse), out RaycastHit hitInfo, interactDistance))
        {
            //Return the game object that we hit
            return hitInfo.collider.gameObject;
        }

        //No Object found - return null
        return null;
    }



}

