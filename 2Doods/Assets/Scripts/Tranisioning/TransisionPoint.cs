using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Point where a character transision can occour
/// </summary>
public class TransisionPoint : MonoBehaviour
{
    //Character Type to Transision to (i.e 3D -> 2D or 2D -> 3D)
    [SerializeField]
    private CharacterStateController.TransisionTypes transisioningTo;
    public CharacterStateController.TransisionTypes TransisionType { get { return transisioningTo; } }

    //Event to Initiate Transision
    public delegate void TransisionEvent(CharacterStateController.TransisionTypes newMode, Transform newCharacterPos, Vector3Int lockedMovementAxies);
    public static event TransisionEvent InitiateTransision;

    //Position to Start the Character at once it has transisioned in to its
    //relevant mode (i.e 3D -> 2D, this var will be where the 2D Character Starts)
    [SerializeField]
    public Transform newCharacterTransform;

    //Locking axies within the 2D rigidbody to stop movement
    //in certain directions so that we don't "fall off"
    //the wall
    [SerializeField]
    private Vector3Int movement2DLockedAxies;

    //Key to Transision
    private KeyCode initiateTransisionButton = KeyCode.E;

    //Tag that is given to player characters
    private const string playerTag = "Player";

    /// <summary>
    /// When the player stays within the trigger allow them
    /// to transision between characters
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //Check that the player is trying to transision
        if (Input.GetKeyDown(initiateTransisionButton))
        {
            //Check that the collider is a character type
            if (other.gameObject.CompareTag(playerTag))
            {
                //If we are transisioning to 3d then set that no movement axies are locked,
                //else lock the 2d axies that are specified
                Vector3Int lockedAxies = transisioningTo == CharacterStateController.TransisionTypes.CharacterTo3D ? Vector3Int.zero : movement2DLockedAxies;

                //Initiate a Transision
                InitiateTransision?.Invoke(transisioningTo, newCharacterTransform, lockedAxies);
            }
        }

    }
}
