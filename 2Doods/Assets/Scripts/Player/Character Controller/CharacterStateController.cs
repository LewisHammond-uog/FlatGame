using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Overall Controller that transisions between 3d and 2d characters.
/// Functions called by a character transision object
/// </summary>
public class CharacterStateController : MonoBehaviour
{
    //Characters
    [SerializeField]
    private GameObject character3D, character2D;

    //State
    public enum TransisionTypes
    {
        CharacterTo3D,
        CharacterTo2D
    }

    private void ChangeCharacterState(TransisionTypes newState, Transform newTransform, Vector3Int lockedMoveAxies)
    {
        //Move character to new position
        GameObject newActiveCharacter = GetNewCharacterObject(newState);

        if(newActiveCharacter != null)
        {
            //Move Character
            newActiveCharacter.transform.position = newTransform.position;
            newActiveCharacter.transform.rotation = newTransform.rotation;

            //Set active character
            SwitchActiveCharacter(newState);
            MoveHeldObject(newState);

            //Lock the Rigidbody axies of the character
            if(lockedMoveAxies != Vector3Int.zero)
            {
                /* 
                 * Locks the movement of the 2D character in certian axies, this is so
                 * that the character cannot pop out of the wall and thus fall out of the world
                 */

                //Get the current character constraints, so that we preserve current rotation constraints
                RigidbodyConstraints character2DConstraints = newActiveCharacter.GetComponent<Rigidbody>().constraints;

                //Reset the movement constraints so that none are left over from the previous wall transision
                character2DConstraints &= ~(RigidbodyConstraints.FreezePositionX) & ~(RigidbodyConstraints.FreezePositionY) & ~(RigidbodyConstraints.FreezePositionZ);

                //Apply movement constratins to how they are set in the edtior
                character2DConstraints = lockedMoveAxies.x == 1 ? character2DConstraints | RigidbodyConstraints.FreezePositionX : character2DConstraints;
                character2DConstraints = lockedMoveAxies.y == 1 ? character2DConstraints | RigidbodyConstraints.FreezePositionY : character2DConstraints;
                character2DConstraints = lockedMoveAxies.z == 1 ? character2DConstraints | RigidbodyConstraints.FreezePositionZ : character2DConstraints;

                //Assign the constrants to the rigidbdy
                newActiveCharacter.GetComponent<Rigidbody>().constraints = character2DConstraints;
            }
        }

    }

    /// <summary>
    /// Gets the new character object based on the character that we
    /// should be switching to
    /// </summary>
    private GameObject GetNewCharacterObject(TransisionTypes newState)
    {
        switch (newState)
        {
            case (TransisionTypes.CharacterTo2D):
            {
                return character2D;
                break;
            }
            case (TransisionTypes.CharacterTo3D):
            {
                return character3D;
                break;
            }
            default:
            {
                return null;
                break;
            }
        }
    }

    /// <summary>
    /// Activate Characters based on an intended states
    /// </summary>
    /// <param name="newState"></param>
    private void SwitchActiveCharacter(TransisionTypes newState)
    {
        //Null Check
        if(!character2D || !character3D)
        {
            return;
        }

        switch (newState)
        {
        case (TransisionTypes.CharacterTo2D):
            character3D.SetActive(false);
            character2D.SetActive(true);
            break;
        case (TransisionTypes.CharacterTo3D):
            character3D.SetActive(true);
            character2D.SetActive(false);
            break;
        default:
            break;
        }
    }

    /// <summary>
    /// Moves the object that the character is holding from
    /// 3D -> 2D or 2D -> 3D
    /// </summary>
    private void MoveHeldObject(TransisionTypes newState)
    {
        switch (newState)
        {
            case TransisionTypes.CharacterTo2D:
                Move3DObjectTo2D();
                break;
            case TransisionTypes.CharacterTo3D:
                Move2DObjectTo3D();
                break;
        }
    }

    /// <summary>
    /// Move an exisitng 3D Object to 2D
    /// </summary>
    private void Move3DObjectTo2D()
    {
        //Get the 3D and 2D Character holding script, if either is invalid,
        //then we can't covert the object
        CharacterObjectHolding3D character3DHoldingComponent = character3D.GetComponent<CharacterObjectHolding3D>();
        CharacterObjectHolding2D character2DHoldingComponent = character2D.GetComponent<CharacterObjectHolding2D>();
        if (!character3DHoldingComponent || !character2DHoldingComponent){ return; }

        //Get the 3D Character's holding object and stop the character holding that object
        HoldableObject3D characterHeldObject = character3DHoldingComponent.HeldHoldableObject;
        if (!characterHeldObject){ return; }

        //Get the 2D object that this 3D Object will convert in to
        GameObject character2DObjPrefab = characterHeldObject.ConvertObject;
        if(character2DObjPrefab == null){ return; }

        //Create the new 2D Object, set it to the 2D Characters pos,
        //and set it as the 2D Held object
        GameObject new2DObj = Instantiate(character2DObjPrefab);
        new2DObj.transform.position = character2D.transform.position;
        new2DObj.transform.rotation = character2D.transform.rotation;
        character2DHoldingComponent.HeldHoldableObject = new2DObj.GetComponent<HoldableObject2D>();

        //Stop the 3D character holding the object
        //and destroy it
        Destroy(character3DHoldingComponent.HeldHoldableObject.gameObject);
        character3DHoldingComponent.HeldHoldableObject = null;
    }

    //TO DO - Change this function
    private void Move2DObjectTo3D()
    {
        //Get the 3D and 2D Character holding script, if either is invalid,
        //then we can't covert the object
        CharacterObjectHolding3D character3DHoldingComponent = character3D.GetComponent<CharacterObjectHolding3D>();
        CharacterObjectHolding2D character2DHoldingComponent = character2D.GetComponent<CharacterObjectHolding2D>();
        if (!character3DHoldingComponent && !character2DHoldingComponent) { return; }

        //Get the 3D Character's holding object and stop the character holding that object
        HoldableObject2D characterHeldObject = character2DHoldingComponent.HeldHoldableObject;
        if (!characterHeldObject) { return; }

        //Get the 3D object that this 2D Object will convert in to
        GameObject character3DObjectPrefab = characterHeldObject.ConvertObject;
        if (character3DObjectPrefab == null) { return; }

        //Create the new 2D Object, set it to the 2D Characters pos,
        //and set it as the 2D Held object
        GameObject new3DObj = Instantiate(character3DObjectPrefab);
        new3DObj.transform.position = character2D.transform.position;
        new3DObj.transform.rotation = character2D.transform.rotation;
        character3DHoldingComponent.HeldHoldableObject = new3DObj.GetComponent<HoldableObject3D>();

        //Stop the 3D character holding the object
        //and destroy it
        Destroy(character2DHoldingComponent.HeldHoldableObject.gameObject);
        character2DHoldingComponent.HeldHoldableObject = null;
    }

    #region Event Sub/UnSub
    private void OnEnable()
    {
        TransisionPoint.InitiateTransision += ChangeCharacterState;
    }

    private void OnDisable()
    {
        TransisionPoint.InitiateTransision += ChangeCharacterState;
    }
    #endregion
}
