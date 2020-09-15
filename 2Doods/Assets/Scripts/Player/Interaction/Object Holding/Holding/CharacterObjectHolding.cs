using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that garentees shared functionality between
/// 2D and 3D Character Object Holding
/// </summary>
public abstract class CharacterObjectHolding : MonoBehaviour
{
    //Object that his character is holding
    protected GameObject heldGameObject;

    //Key for pickup/drop
    protected KeyCode pickupObjectKey = KeyCode.F;

    //Abstact Functions for Pickup/Drop/Update Held objects
    protected abstract void PickupObject(GameObject objectToPickup);
    protected abstract void DropHeldObject();
    protected abstract void UpdateHeldObject();

}
