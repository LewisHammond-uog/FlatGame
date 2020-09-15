using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class for objects that are used for events
/// Class stores infomation about the event type and
/// has abstract functions for 
/// </summary>
public class EventObject : MonoBehaviour
{
    //Type of event
    public enum EventTypes
    {
        EventWallDestruction,

        EventTypesCount
    }

    //Type of event that is object is for
    [SerializeField]
    public EventTypes objectEventType;

    //Abstract function for trigger event
    public virtual void TriggerEvent()
    {
        //Do Nothing
    }

}
