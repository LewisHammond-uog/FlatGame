using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterObjectHolding2D : CharacterObjectHolding
{

    #region OBJECT HOLDING PROPERTIES
    /////OBJECT HOLDING PROPERTIES//////
    //Radius that we can pick objects up from
    [Header("Object Holding")]
    [Min(0)]
    [SerializeField]
    protected float pickupDistance;
    //Distance that the object is held out in front of us
    //when holding
    [Min(0)]
    [SerializeField]
    protected float objectHoldDistance;
    //Smoothing applied when moving the held object
    [Min(0.1f)]
    [SerializeField]
    protected float objectMovementSmoothing = 1.0f;
    #endregion

    private bool collidingWithHeldObject = false;

    //Property to get the held object script. 
    //Get Returns the held object script as the gameobject we are holding doesn't
    //Set properly picks the game object attached to the holdable script, as well as
    //dropping the current gameobject
    public HoldableObject2D HeldHoldableObject
    {
        get
        {
            if (heldGameObject)
            {
                return heldGameObject.GetComponent<HoldableObject2D>();
            }
            return null;
        }
        set
        {
            if (heldGameObject != null)
            {
                DropHeldObject();
            }
            if (value != null)
            {
                PickupObject(value.gameObject);
            }
        }
    }

    private void LateUpdate()
    {

        if (Input.GetKeyDown(pickupObjectKey))
        {
            if (heldGameObject == null)
            {
                //Get the object we are looking at (if it is a pickupable)
                GameObject selectedPickupable = GetClosestPickupable();
                if (selectedPickupable != null)
                {
                    //Check that distance is less than the pickup distance
                    if (Vector3.Distance(selectedPickupable.transform.position, transform.position) < pickupDistance)
                    {
                        //Pickup the object
                        PickupObject(selectedPickupable);
                    }
                }
            }
            else
            {
                //Make sure that we are not colliding with the 
                //object we are about to drop
                if (!collidingWithHeldObject)
                {
                    //Drop the currently held object
                    DropHeldObject();
                }
            }
        }

        //Update the held object if we have one
        if (heldGameObject != null)
        {
            UpdateHeldObject();
        }
    }

    /// <summary>
    /// Pickup a GameObject
    /// </summary>
    protected override void PickupObject(GameObject objectToPickup)
    {
        //Set the held object
        heldGameObject = objectToPickup;

        //Disable Collider - temp
        if (heldGameObject.GetComponent<Collider>())
        {
            heldGameObject.GetComponent<Collider>().isTrigger = true;
        }

        //Remove the physics world interactions from 
        //the pickupable
        if (objectToPickup.GetComponent<Rigidbody>())
        {
            Rigidbody objRb = objectToPickup.GetComponent<Rigidbody>();
            objRb.useGravity = false;
            objRb.isKinematic = true;
        }
    }

    /// <summary>
    /// Drop the held GameObject
    /// </summary>
    protected override void DropHeldObject()
    {
        //Enable Collider - temp
        if (heldGameObject.GetComponent<Collider>())
        {
            heldGameObject.GetComponent<Collider>().isTrigger = false;
        }

        //Remove the physics world interactions from 
        //the pickupable
        if (heldGameObject.GetComponent<Rigidbody>())
        {
            Rigidbody objRb = heldGameObject.GetComponent<Rigidbody>();
            objRb.useGravity = true;
            objRb.isKinematic = false;
        }

        //Set held object to null
        heldGameObject = null;
    }

    /// <summary>
    /// Updates the held object's position based on where the player is
    /// </summary>
    protected override void UpdateHeldObject()
    {
        //Null Check Held Object
        if(heldGameObject == null)
        {
            return;
        }

        //Get the current position of the player
        //and held object
        Vector3 playerCurrentPos = transform.position;
        Vector3 heldObjectCurrentPos = heldGameObject.transform.position;

        //Get the target position
        //Get the actual right of the object - the player character flips it's direction
        //by fliping it's scale
        Vector3 rightTransform = (gameObject.transform.right * transform.localScale.x);

        //Get the target position
        Vector3 heldObjectTargetPos = playerCurrentPos + (rightTransform * objectHoldDistance);

        //Move the held object towards it's target position
        heldGameObject.transform.position = Vector3.Lerp(heldObjectCurrentPos, heldObjectTargetPos, Time.deltaTime * objectMovementSmoothing);
    }

    private GameObject GetClosestPickupable()
    {
        //Find all Holdable 2D Objects
        HoldableObject2D[] holdable2DObjects = FindObjectsOfType<HoldableObject2D>();

        //Loop through all holdable objects and get the closest one
        float closestDistance = Mathf.Infinity;
        GameObject closestHoldable = null;
        foreach(HoldableObject2D currentHoldable in holdable2DObjects)
        {
            //Get the current objects position
            float currentHoldableDist = Vector3.Distance(transform.position, currentHoldable.transform.position);

            //Check if this is closer than our current lowest distance
            //then assign it to the new closest
            if(currentHoldableDist < closestDistance)
            {
                closestDistance = currentHoldableDist;
                closestHoldable = currentHoldable.gameObject;
            }
        }

        return closestHoldable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (heldGameObject != null)
        {
            if (other.gameObject == HeldHoldableObject.gameObject)
            {
                collidingWithHeldObject = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (heldGameObject != null)
        {
            if (other.gameObject == HeldHoldableObject.gameObject)
            {
                collidingWithHeldObject = false;
            }
        }
    }


}

