using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for button that turns on/off 2d wall sections
/// </summary>
public class WallButtonController : MonoBehaviour
{

    [Header("Wall Items/Options")]
    //Whether we should use random linking (i.e buttons and linked groups are 
    //randomised every time). If this option is disabled then item 1 in the
    //wallGroups will be linked with button 1 every time.
    [SerializeField]
    private bool randomiseButtonLinks = true;
    //List of Buttons
    [SerializeField]
    private List<WallButton> buttons;
    //Array of wall items - that will be turned on/off
    [SerializeField]
    private List<GameObject> wallGroups;


    //Dictonary of which buttons enable/disable which
    //wall groups
    private Dictionary<WallButton, GameObject> buttonWallPairs;

    // Start is called before the first frame update
    void Start()
    {
        //Check that buttons and wall groups have the same length
        if (wallGroups.Count != buttons.Count)
        {
            Debug.LogWarning("There are not the same number of buttons and wall items - this will cause problems");
        }

        //Assign the wall button pairs using whatever method the user has choosen
        buttonWallPairs = randomiseButtonLinks ? RandomButtonLink() : LinearButtonLink();

        //Give each button it's linked object
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].AssignedWallGroup = buttonWallPairs[buttons[i]];
        }

    }

    private Dictionary<WallButton, GameObject> RandomButtonLink()
    {
        //Create Dictonary
        Dictionary<WallButton, GameObject> randomLinkDict = new Dictionary<WallButton, GameObject>();

        //Create temp lists so that we can remove items as they are assigned
        List<WallButton> unassignedButtons = new List<WallButton>(buttons);
        List<GameObject> unassignedWallGroups = new List<GameObject>(wallGroups);

        //Loop untill all buttons are assigned
        while (unassignedButtons.Count > 0)
        {
            //Choose Random indexes for button and wallGroup
            int randomButtonIndex = Random.Range(0, unassignedButtons.Count);
            int randomWallIndex = Random.Range(0, unassignedButtons.Count);

            //Link the button and wallGroup together
            randomLinkDict.Add(unassignedButtons[randomButtonIndex], unassignedWallGroups[randomWallIndex]);

            //Remove from the unassigned list
            unassignedButtons.RemoveAt(randomButtonIndex);
            unassignedWallGroups.RemoveAt(randomWallIndex);
        }

        //Return Dictonary
        return randomLinkDict;
    }

    /// <summary>
    /// Link the buttons and wall items linearly
    /// </summary>
    private Dictionary<WallButton, GameObject> LinearButtonLink()
    {
        //Create Dictonary
        Dictionary<WallButton, GameObject> linearLinkDict = new Dictionary<WallButton, GameObject>();

        //Loop through all of the buttons and link them to the
        //wall items 1 to 1 in the lists
        for (int i = 0; i < buttons.Count; i++)
        {
            linearLinkDict.Add(buttons[i], wallGroups[i]);
        }

        //Return the dictonary
        return linearLinkDict;
    }
}
