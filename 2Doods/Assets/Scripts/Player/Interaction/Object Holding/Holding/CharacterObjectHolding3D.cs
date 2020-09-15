using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObjectHolding3D : CharacterObjectHolding
{
    //Component for player interaction
    PlayerInteraction3D playerInteraction;

    //Property to get the held object script. 
    //Get Returns the held object script as the gameobject we are holding doesn't
    //Set properly picks the game object attached to the holdable script, as well as
    //dropping the current gameobject
    public HoldableObject3D HeldHoldableObject
    {
        get
        {
            if (heldGameObject)
            {
                return heldGameObject.GetComponent<HoldableObject3D>();
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
    //Speed for the object to follow at when held
    [Min(0.1f)]
    [SerializeField]
    protected float objectMoveSpeed = 1.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Get the interaction component attached to the player
        playerInteraction = GetComponent<PlayerInteraction3D>();
        if (!playerInteraction)
        {
            Debug.LogWarning("CharacterObjectHolding3D does not have interaction component - this will cause errors for pickup and interaction");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pickupObjectKey))
        {
            if (heldGameObject == null)
            {
                //Get the object we are looking at (if it is a pickupable)
                GameObject selectedPickupable = GetLookingAtPickupable();
                if(selectedPickupable != null)
                {
                    //Pikcup the object
                    PickupObject(selectedPickupable);
                }
            }
            else
            {
                //Drop the currently held object
                DropHeldObject();
            }
        }

        //Update the held object position if we are
        //holding one
        if(heldGameObject != null)
        {
            UpdateHeldObject();
        }
    }

    /// <summary>
    /// Get the object that the player is looking at
    /// </summary>
    private GameObject GetLookingAtPickupable()
    {
        //Get the object that we are looking at from the interaction component
        GameObject lookingAtObject = playerInteraction.LookingAtObject;

        //Check that that object has a holdable component if it does - return it,
        return lookingAtObject?.GetComponent<HoldableObject3D>() ? lookingAtObject : null;
    }

    /// <summary>
    /// Update the postion of the object we are holding
    /// so that it moves with the character
    /// </summary>
    protected override void UpdateHeldObject()
    {
        //Null check the held object
        if (!heldGameObject)
        {
            return;
        }

        //Try and get the rigidbody from the held object so that we can
        //move it around with physics
        Rigidbody heldRigidbody = heldGameObject.GetComponent<Rigidbody>();
        if (!heldRigidbody)
        {
            Debug.LogWarning("Trying to carry an object that doesn't have a rigidbody");
            return;
        }

        //Get the current position of the player
        //and held object
        Vector3 playerCurrentPos = transform.position;
        Vector3 heldObjectCurrentPos = heldGameObject.transform.position;

        //Calculate the target positon of the held object
        Vector3 heldObjectTargetPos = playerCurrentPos + (gameObject.transform.forward * objectHoldDistance);

        //Calculate the position that we should move to next frame
        Vector3 movePosition = Vector3.Lerp(heldObjectCurrentPos, heldObjectTargetPos, Time.deltaTime * objectMoveSpeed);
        
        //Only move to the move position if we do not have something blocking our movement
        //this is so we cannot clip the object through a wall and get it stuck
        if (!Physics.Linecast(heldObjectCurrentPos, heldObjectTargetPos))
        {
            heldRigidbody.MovePosition(movePosition);
        }

    }

    /// <summary>
    /// Pickup a GameObject
    /// </summary>
    protected override void PickupObject(GameObject objectToPickup)
    {
        //Set the held object
        heldGameObject = objectToPickup;

        //Remove the physics world interactions from 
        //the pickupable
        if (objectToPickup.GetComponent<Rigidbody>())
        {
            Rigidbody objRB = objectToPickup.GetComponent<Rigidbody>();
            objRB.isKinematic = false;
            objRB.useGravity = false;
        }

    }

    /// <summary>
    /// Drop the held GameObject
    /// </summary>
    protected override void DropHeldObject()
    {
        //Null Check the held object
        if(heldGameObject != null)
        {
            //Readd the world physics interactions, if this object
            //had a rigidbody
            //Remove the physics world interactions from 
            //the pickupable
            if (heldGameObject.GetComponent<Rigidbody>())
            {
                Rigidbody objRb = heldGameObject.GetComponent<Rigidbody>();
                objRb.isKinematic = false;
                objRb.useGravity = true;
            }

            //Remove as held object
            heldGameObject = null;
        }
    }

}
