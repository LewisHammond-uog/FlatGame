using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLock : MonoBehaviour
{
    [SerializeField]
    private GameObject attachedObject;

    [SerializeField]
    private int requiredKeyID = -1;

    /// <summary>
    /// Attempt to unlock this lock with the given key
    /// </summary>
    /// <param name="attemptKey">Dictonary of keys to test</param>
    /// <returns>Key used to succesfully unlock the lock, null if no success</returns>
    public bool AttemptUnlock(KeyInfo attemptKey)
    {
        //Check if the keyid is the key id we require
        if (attemptKey.keyId == requiredKeyID)
        {
            //Unlock Door and return true
            Unlock();
            return true;
        }

        //Player does not have the key
        return false;
    }

    /// <summary>
    /// Unlock the item that this lock is attached to
    /// </summary>
    private void Unlock()
    {
        //Destory the attached object
        Destroy(attachedObject);
    }
}
