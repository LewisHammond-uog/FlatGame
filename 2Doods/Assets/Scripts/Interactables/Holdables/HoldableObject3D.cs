using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class HoldableObject3D : HoldableObject
{
    //Store the player object
    private PlayerInteraction3D playerInteraction;
    private CharacterObjectHolding3D playerHolding;

    //Object Highlighting functionality
    private ObjectHighlighting pickupHighlight;

    //2D Object that this object converts in to when moving
    //2D -> 3D
    [SerializeField]
    private GameObject convertObject;
    //Property for the convert object, only return the game object if 
    //it is a valid 2D Holdable Object
    public GameObject ConvertObject
    {
        get
        {
            if (convertObject.GetComponent<HoldableObject2D>())
            {
                return convertObject;
            }
            else
            {
                return null;
            }
        }
    }


    private void Start()
    {
        //Get the player
        playerInteraction = FindObjectOfType<PlayerInteraction3D>();
        playerHolding = FindObjectOfType<CharacterObjectHolding3D>();

        //Setup highlight
        pickupHighlight = gameObject.AddComponent<ObjectHighlighting>();
    }

    private void Update()
    {
        //Get if we are being looked at or not and send info to the shader
        //for drawing outline
        if (playerHolding && playerInteraction)
        {
            if (playerHolding.gameObject.activeInHierarchy && playerHolding.HeldHoldableObject == null)
            {
                pickupHighlight.SetDrawOutline(playerInteraction.LookingAtObject == gameObject);
            }
            else
            {
                pickupHighlight.SetDrawOutline(false);
            }
        }
        else
        {
            pickupHighlight.SetDrawOutline(false);
        }
    }

  
}
