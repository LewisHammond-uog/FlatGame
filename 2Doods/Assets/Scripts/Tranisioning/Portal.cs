using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Portal : MonoBehaviour
{
    //The transision point that this portal is relevent to
    [SerializeField]
    private TransisionPoint attachedTransision;

    //Sprite Renderer that 
    [SerializeField]
    private SpriteRenderer portalRenderer;
    [SerializeField] 
    private BoxCollider portalCollider;

    //Sprites for different portal types
    [SerializeField]
    private Sprite transisionTo3DSprite, transisionTo2DSprite;

    // Start is called before the first frame update
    void Start()
    {
        //Set the portal sprite based on the type of transision
        //that we are attached to
        switch (attachedTransision.TransisionType)
        {
            case (CharacterStateController.TransisionTypes.CharacterTo2D):
                portalRenderer.sprite = transisionTo2DSprite;
                break;
            case (CharacterStateController.TransisionTypes.CharacterTo3D):
                portalRenderer.sprite = transisionTo3DSprite;
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// Switch the rendering and collision of the portal on or off depending on it's
    /// state
    /// </summary>
    /// <param name="newMode">The Portals new mode</param>
    /// <param name="newCharacterPos">The New Characters Position</param>
    /// <param name="lockedMovementAxies">The axies that are locked for the character</param>
    private void SwitchPortalState(CharacterStateController.TransisionTypes newMode, Transform newCharacterPos, Vector3Int lockedMovementAxies)
    {
        //If we have just transisioned to our type disable transision
        //from the portal controller, which sould be our parent
        bool parentShouldBeEnabled = (newMode != attachedTransision.TransisionType);

        portalRenderer.enabled = parentShouldBeEnabled;
        portalCollider.enabled = parentShouldBeEnabled;
    }

    #region Event Subs/UnSubs
    private void OnEnable()
    {
        TransisionPoint.InitiateTransision += SwitchPortalState;
    }
    private void OnDisable()
    {
        TransisionPoint.InitiateTransision -= SwitchPortalState;
    }
    #endregion
}
