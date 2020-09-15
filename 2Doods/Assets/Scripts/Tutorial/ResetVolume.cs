using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetVolume : MonoBehaviour
{
    private const string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            //Reset the scene if we "die"
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
