using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a world space tool tip. Shows text when player
/// is in range and faces the player at all times
/// </summary>
public class WorldSpaceTooltip : MonoBehaviour
{
    [SerializeField] private GameObject player;

    //Store the rect transform that we should rotate towards the player
    [SerializeField]
    private RectTransform toolTipTransform;

    //Directions to lock movement in                            //Lock Y by default
    [SerializeField] private Vector3Int lockMovementDirections = new Vector3Int(0,1,0);


    // Update is called once per frame
    private void Update()
    {

        if (player)
        {
            if (toolTipTransform)
            {

                //Get the direction to the player
                //Set 0 y rotation so that we don't move in the y direction. Keeping the tool tip moving in the X/Y Plane
                Vector3 direction = (transform.position - player.transform.position).normalized;

                //Lock Movement Directions
                direction.x = lockMovementDirections.x == 1 ? direction.x = 0f : direction.x;
                direction.y = lockMovementDirections.y == 1 ? direction.y = 0f : direction.y;
                direction.z = lockMovementDirections.z == 1 ? direction.z = 0f : direction.z;

                //Rotate so that the tooltip is facing the player
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
