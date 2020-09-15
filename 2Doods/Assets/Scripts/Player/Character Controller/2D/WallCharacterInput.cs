using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(WallCharacterController))]
public class WallCharacterInput : MonoBehaviour
{
    private WallCharacterController characterController;
    private bool jumpPressed;

    private void Awake()
    {
        characterController = GetComponent<WallCharacterController>();
    }


    private void Update()
    {
        if (!jumpPressed)
        {
            // Read the jump input in Update so button presses aren't missed.
            jumpPressed = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }


    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // Pass all parameters to the character control script.
        characterController.Move(h, jumpPressed);
        jumpPressed = false;
    }
}
