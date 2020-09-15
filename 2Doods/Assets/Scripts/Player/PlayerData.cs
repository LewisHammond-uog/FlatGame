using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Water;

/// <summary>
/// Class stores data about the player
/// (e.g held keys etc.)
/// </summary>
public class PlayerData : MonoBehaviour
{
    //Event for the player dying
    public delegate void PlayerEvent();
    public static event PlayerEvent PlayerKilled;

    //Property - Dictionary for held keys <keyId, keyInfo>
    public Dictionary<int, KeyInfo> PlayerHeldKeys { private set; get; }

    private const float playerStartHealth = 100;
    private const float playerWaterDamage = 4f; //Amount of damaege that water does per second
    private float playerHealth;

    //String for a water volume
    private const string waterVolumeTag = "WaterDeathVolume";

    private void Start()
    {
        //Init held keys
        PlayerHeldKeys = new Dictionary<int, KeyInfo>();

        //Init Health
        playerHealth = playerStartHealth;
    }

    private void Update()
    {
        //Check at our health is > 0, otherwise kill us
        if (playerHealth <= 0)
        {
            PlayerKilled?.Invoke();
        }
    }

    private void DamagePlayer(float amount)
    {
        playerHealth -= amount;
    }

    /// <summary>
    /// Gets if the player is holding a key
    /// </summary>
    /// <param name="checkKeyId"></param>
    /// <returns></returns>
    public bool PlayerHoldingKey(int checkKeyId)
    {
        //Check if the Dictionary contains the given key ID
        if (PlayerHeldKeys.ContainsKey(checkKeyId))
        {
            //Return True - we are holding the key
            return true;
        }

        //Held Keys does not contain the keyId
        return false;
    }

    /// <summary>
    /// Add a key to the list of stored keys
    /// </summary>
    /// <param name="keyToAdd"></param>
    private void AddPlayerKey(KeyInfo keyToAdd)
    {
        //Check that we don't already have the key
        if (!PlayerHeldKeys.ContainsKey(keyToAdd.keyId))
        {
            //Add the key to the Dictionary
            PlayerHeldKeys.Add(keyToAdd.keyId, keyToAdd);
        }
    }

    /// <summary>
    /// Use a key that the player has held
    /// </summary>
    /// <param name="usedKey">Key to use</param>
    private void UseKey(KeyInfo usedKey)
    {
        //Check that we have the key
        if (PlayerHeldKeys.ContainsKey(usedKey.keyId))
        {
            //If the key should be removed on use then remove it
            if (usedKey.removeOnUse)
            {
                PlayerHeldKeys.Remove(usedKey.keyId);
            }
        }
    }

    #region Event Subs/Unsubs
    private void OnEnable()
    {
        PlayerInteraction.KeyPickedUp += AddPlayerKey;
        PlayerInteraction.KeyUsed += UseKey;
        WaterDamage.PlayerDamaged += DamagePlayer;
    }
    private void OnDisable()
    {
        PlayerInteraction.KeyPickedUp -= AddPlayerKey;
        PlayerInteraction.KeyUsed -= UseKey;
        WaterDamage.PlayerDamaged -= DamagePlayer;
    }
    #endregion
}
