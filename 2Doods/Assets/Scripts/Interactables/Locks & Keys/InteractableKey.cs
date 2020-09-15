using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct stores infomation about a key
/// </summary>
[System.Serializable]
public struct KeyInfo
{
    public int keyId; //ID of the Key
    public bool removeOnUse; //Remove the key from the players inventory when it unlocks a lock
}

/// <summary>
/// Script for a key in the world
/// </summary>
public class InteractableKey : MonoBehaviour
{
    [SerializeField]
    private KeyInfo keyInfo;
    public KeyInfo KeyInfo { get { return keyInfo; } }
}
