using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : InteractableButton
{

    //Wall Object Group (objects that it toggles on/off) 
    //that this button is linked to
    private GameObject wallGroup = null;
    //Only allow setting of the wall group if one is not already
    //assigned
    public GameObject AssignedWallGroup
    {
        set
        {
            if(wallGroup == null)
            {
                wallGroup = value;
            }
        }
        get { return wallGroup; }
    }

    protected override void TriggerInteraction()
    {
        //Call base function - do visuals
        base.TriggerInteraction();

        if (wallGroup != null)
        {
            //Toggle the assigned wallGroup's active status
            wallGroup.SetActive(!wallGroup.activeSelf);
        }
    }
}
