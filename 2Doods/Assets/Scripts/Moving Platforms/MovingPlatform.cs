using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MovingPlatform : MonoBehaviour
{
    private FirstPersonController character;

    private Vector3 moveDirection;
    private Vector3 prevPos;

    private void Update()
    {
        moveDirection = transform.position - prevPos;
        prevPos = transform.position;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!character)
        {
            character = collision.gameObject.GetComponent<FirstPersonController>();
        }
        else
        {
            character.externalMoveSpeed = moveDirection;
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (character)
        {
            if (collision.gameObject == character.gameObject)
            {
                character.externalMoveSpeed = Vector3.zero;
                character = null;
            }
        }
    }
}
