using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the water damaging the player
/// </summary>
public class WaterDamage : MonoBehaviour
{
    //Event triggered when the player is damaged
    public delegate void WaterDamageEvent(float damage);
    public static event WaterDamageEvent PlayerDamaged;

    [SerializeField] private GameObject waterPlane;

    private const float waterDamage = 4.5f; //Amount of damage the water does per second

    //Tag that player objects have
    private const string playerTag = "Player";

    //Damage when the player stays in the trigger
    private void OnTriggerStay(Collider other)
    {
        //Check we are coillding with the player
        if (other.CompareTag(playerTag))
        {
            //If the top of our player is under the top of the water,
            //then damage us
            Vector3 playerPos = other.gameObject.transform.position;
            float playerMaxY = other.bounds.max.y;
            float waterY = waterPlane.transform.position.y;

            if (playerMaxY < waterY)
            {
                //Damage the player we are under water
                PlayerDamaged?.Invoke(waterDamage * Time.deltaTime);
            }

        }
    }
}