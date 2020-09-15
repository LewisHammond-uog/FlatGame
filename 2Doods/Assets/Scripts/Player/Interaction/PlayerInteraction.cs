using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that deals with player interaction shared between 2D and 3D 
/// </summary>
public abstract class PlayerInteraction : MonoBehaviour
{
    //Event for picking up/using keys
    public delegate void PlayerKeyEvent(KeyInfo key);
    public static event PlayerKeyEvent KeyPickedUp;
    public static event PlayerKeyEvent KeyUsed;


    /// <summary>
    /// Checks if a given game object is a valid key
    /// </summary>
    /// <param name="keyObj">Potential Key Object</param>
    protected void CheckForKeyPickup(GameObject keyObj)
    {
        InteractableKey interactedKey = keyObj.GetComponent<InteractableKey>();
        if (interactedKey)
        {
            //Add key to our list - it's a struct so it will copy
            KeyPickedUp?.Invoke(interactedKey.KeyInfo);

            //Destory Key Object
            Destroy(interactedKey.gameObject);
        }
    }

    /// <summary>
    /// Check for lock interaction and trigger unlock if appropriate
    /// </summary>
    /// <param name="lockObj">Potential Lock Object</param>
    protected void CheckForLockInteraction(GameObject lockObj)
    {
        //Check are trying to unlock a lock
        InteractableLock interactedLock = lockObj.GetComponent<InteractableLock>();
        if (interactedLock)
        {
            //Get the player data component
            PlayerData playerDataComponent = FindObjectOfType<PlayerData>();
            //Early out if we do not have player data, as we can't check what keys we have
            if (!playerDataComponent)
            {
                return;
            }

            //Unlock the lock and call event to use key
            //Loop through our keys and attempt to unlock the lock
            if (playerDataComponent.PlayerHeldKeys != null)
            {
                foreach (var key in playerDataComponent.PlayerHeldKeys)
                {
                    //Check if this key unlocks the lock - if it does
                    //then call the event to update that it has been used
                    if (interactedLock.AttemptUnlock(key.Value))
                    {
                        KeyUsed?.Invoke(key.Value);
                    }
                }
            }
        }
    }

    //Entering the trigger for picking up a key
    protected void OnTriggerEnter(Collider other)
    {
        //Call Functions to check for key or lock interaction
        CheckForKeyPickup(other.gameObject);
        CheckForLockInteraction(other.gameObject);
    }
}
