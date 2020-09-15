using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for interactable buttons in the world
/// The object that this script is attached to should have a trigger
/// to detect if the player is in range
/// </summary>
public class InteractableButton : MonoBehaviour
{
    protected bool playerInRange;
    protected PlayerInteraction3D playerInteractionComponent;

    //Player Interaction Key
    private KeyCode buttonInteractionKey = KeyCode.E;

    //Button Types
    protected enum ButtonType
    {
        ButtonTypePress,
        ButtonTypeToggle
    }

    [Header("Button Settings")]
    //Type of button that this is 
    [SerializeField]
    protected ButtonType buttonType;
    //Offset for button to be when depressed
    [SerializeField]
    private Vector3 buttonDepressOffset;
    //Number of seconds button is depressed for, only used in 
    //ButtonTypePress mode
    [SerializeField]
    private float buttonDepressTime = 2f;
    //Store if the button is currently depressed
    private bool isButtonDepressed;

    //Highlight component used to highlight the button when looked at
    private ObjectHighlighting buttonHighlighting;

    private void Start()
    {
        //Create button highlighting
        buttonHighlighting = gameObject.AddComponent<ObjectHighlighting>();
    }

    protected void Update()
    {

        //Check that we have a player in range
        if (playerInRange)
        {
            //Check if the player is looking at this object and highlight
            bool playerLookingAtButton = playerInteractionComponent.LookingAtObject == this.gameObject;
            buttonHighlighting.SetDrawOutline(playerLookingAtButton);

            //Check if the player - that is in range - has pressed the button interaction
            //key
            if (Input.GetKeyDown(buttonInteractionKey))
            {
                //Check if we are the object being looked at by the player and the button
                //is not depressed
                if (playerLookingAtButton)
                {
                    //Then trigger the interaction from this object
                    TriggerInteraction();
                }
            }
        }
    }

    /// <summary>
    /// Trigger the buttons interaction
    /// For this button type only change the visual state
    /// </summary>
    protected virtual void TriggerInteraction()
    {
        switch (buttonType)
        {
            case ButtonType.ButtonTypePress:
                //Depress the button
                if (!isButtonDepressed)
                {
                    StartCoroutine(DepressButtonForSeconds(buttonDepressTime));
                }
                break;
            case ButtonType.ButtonTypeToggle:
                //Toggle the state of the button
                if (!isButtonDepressed)
                {
                    //Button is not depressed - depress it
                    transform.localPosition += buttonDepressOffset;
                    isButtonDepressed = true;
                }
                else
                {
                    //Button is depressed - revert it to normal
                    transform.localPosition -= buttonDepressOffset;
                    isButtonDepressed = false;
                }
                break;
        }
    }

    /// <summary>
    /// Depress the button for a given number of seconds
    /// </summary>
    /// <param name="seconds">Number of seconds to depress button for</param>
    /// <returns></returns>
    private IEnumerator DepressButtonForSeconds(float seconds)
    {
        //Add the offset to our position, wait, then return the button
        //to it's original position
        transform.position += buttonDepressOffset;
        isButtonDepressed = true;
        yield return new WaitForSeconds(seconds);
        transform.position -= buttonDepressOffset;
        isButtonDepressed = false;
    }

    //Check if anything has entered the trigger, if it is the
    //player then set the flag, use on trigger enter vs on trigger stay
    //as there will be a lot of geomerty that will be staying but is not the 
    //player
    private void OnTriggerEnter(Collider other)
    {
        //Check if it is the player by checking if it has the interaction
        //component - as this this is the component that we want to use
        //for interaction with the button
        PlayerInteraction3D potentialPlayer = other.gameObject.GetComponent<PlayerInteraction3D>();
        if (potentialPlayer)
        {
            playerInRange = true;
            playerInteractionComponent = potentialPlayer;
        }
    }

    //If the player moves out of range then set vars accordingly
    private void OnTriggerExit(Collider other)
    {
        //Check that is is the player that is leaving
        if (other.gameObject.GetComponent<PlayerInteraction3D>())
        {
            //Unset player in range and remove stored interaction compnent
            playerInRange = false;
            playerInteractionComponent = null;
        }
    }
}
