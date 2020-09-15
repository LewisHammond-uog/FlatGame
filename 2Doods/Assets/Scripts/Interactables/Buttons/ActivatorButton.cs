using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorButton : InteractableButton
{
    //List of events to activate
    [SerializeField] private EventObject[] activatorEvents;

    /// <summary>
    /// Trigger the events for this button press
    /// </summary>
    protected override void TriggerInteraction()
    {
        base.TriggerInteraction();

        //When we are activated call the events
        foreach (EventObject activatorEvent in activatorEvents)
        {
            activatorEvent.TriggerEvent();
        }
    }


}
