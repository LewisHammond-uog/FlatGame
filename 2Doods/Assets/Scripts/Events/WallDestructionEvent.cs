using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestructionEvent : MonoBehaviour
{

    [SerializeField]
    private GameObject destructableWall;

    /// <summary>
    /// Triggers the event
    /// </summary>
    public void TriggerEvent()
    {
        //Debug Destroy the wall
        if(destructableWall != null) {
            Destroy(destructableWall);
        }
    }

    //Wait until we have a collision with the sphere
    private void OnTriggerEnter(Collider other)
    {
        //Check that they object is a 2d holdable and an event object
        if (other.gameObject.GetComponent<HoldableObject2D>() &&
            other.gameObject.GetComponent<EventObject>())
        {
            //Check that event is of the right type
            if (other.gameObject.GetComponent<EventObject>().objectEventType == EventObject.EventTypes.EventWallDestruction)
            {
                //Trigger Event
                TriggerEvent();
            }
        }
    }
}
