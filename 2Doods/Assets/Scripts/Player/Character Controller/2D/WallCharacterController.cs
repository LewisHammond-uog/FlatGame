using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCharacterController : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 10f;      // The fastest the player can travel in the x axis.
    [SerializeField] private float airMoveModifier = 0.5f;  //Amount of muliply the move speed by while airborne 
    [SerializeField] private float jumpForce = 400f;        // Amount of force added when the player jumps.
    [SerializeField] private bool allowAirControl = false;  // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask groundLayerMask;     // A mask determining what is ground to the character

    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    private const float groundCheckRadius = 0.02f; // Radius of the overlap circle to determine if grounded
    private bool isGrounded;            // Whether or not the player is grounded.
    private Animator characterAnimator;            // Reference to the player's animator component.
    private Rigidbody characterRb; //Characters Rigidbody
    private bool facingRight = true;  // For determining which way the player is currently facing.
    private BoxCollider playerCollider; //Collider for the player object

    //Variables to store links to animator values rather than using strings
    private readonly int animatorGroundProperty = Animator.StringToHash("Ground");
    private readonly int animatorSpeedProperty = Animator.StringToHash("Speed");
    private readonly int animatorVSpeedProperty = Animator.StringToHash("vSpeed");

    //Property of the size of the collider
    private float PlayerColliderSize { get { return Mathf.Max(playerCollider.size.x, playerCollider.size.y, playerCollider.size.z) * .5f; } }

    private void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        characterAnimator = GetComponent<Animator>();
        characterRb = GetComponentInChildren<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
    }


    private void FixedUpdate()
    {
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
        }

        //Set Animator Values
        characterAnimator.SetBool(animatorGroundProperty, isGrounded);
        characterAnimator.SetFloat(animatorVSpeedProperty, characterRb.velocity.y);
    }


    public void Move(float move, bool jump)
    {

        //only control the player if grounded or airControl is turned on
        if (isGrounded || allowAirControl)
        {
            // The animatorSpeedProperty animator parameter is set to the absolute value of the horizontal input.
            characterAnimator.SetFloat(animatorSpeedProperty, Mathf.Abs(move));

            // Check if adding the new velocity of the player will force
            // the player outside world bounds. If this is the case then set our
            // x and z velocity to 0 so that we don't go outside the bounds
            Vector3 newVelocity = new Vector3(0f, characterRb.velocity.y, 0f) + ((move * maxMoveSpeed) * transform.right);
            if (!PlayerInWorldNextFrame(newVelocity))
            {
                newVelocity = new Vector3(0f, characterRb.velocity.y, 0f);
            }

            //If the character is in the air then reduce their x/z velocity by a modifier
            if (!isGrounded)
            {
                newVelocity.x *= airMoveModifier;
                newVelocity.z *= airMoveModifier;
            }

            characterRb.velocity = newVelocity;


            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (isGrounded && jump && characterAnimator.GetBool(animatorGroundProperty))
        {
            // Add a vertical force to the player.
            isGrounded = false;
            characterAnimator.SetBool(animatorGroundProperty, false);
            characterRb.AddForce(new Vector2(0f, jumpForce));
        }


        //Set animator properties
        characterAnimator.SetBool(animatorGroundProperty, isGrounded);
        characterAnimator.SetFloat(animatorSpeedProperty, Mathf.Abs(move));
    }

    /// <summary>
    /// Checks if the player will be within the world bounds after it's
    /// movement this frame
    /// (i.e that it won't make the player fall out of the 2DWorld)
    /// </summary>
    /// <returns></returns>
    private bool PlayerInWorldNextFrame(Vector3 velocity)
    {
        //Get the characters position, up and right and store it so we only access it once
        //rather than requesting it every time we are calculating a position
        Vector3 characterPos = transform.position;
        Vector3 characterUp = transform.up;
        Vector3 characterRight = transform.right;

        //Get the left and right bounds plus the current velocity,
        //i.e the bounds of the player next frame
        Vector3 leftPos = characterPos + (characterRight * PlayerColliderSize) + (velocity * Time.deltaTime);
        Vector3 rightPos = characterPos + (-characterRight * PlayerColliderSize) + (velocity * Time.deltaTime);

        //Check if the left and right are not off the world. Check from the left/right position and cast downards
        //and look for anything that qualifies as ground for the 2d Character
        bool inWorldLeft = !Physics.Raycast(leftPos, -characterUp, Mathf.Infinity, groundLayerMask);
        bool inWorldRight = !Physics.Raycast(rightPos, -characterUp, Mathf.Infinity, groundLayerMask);

        //If there is not a valid 2d world object below us then return false as the players
        //bounds would not be within the world bounds
        return !inWorldLeft && !inWorldRight;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
