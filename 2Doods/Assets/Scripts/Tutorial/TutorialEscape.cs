using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Portal whihc escapes the tutorial section
/// </summary>
public class TutorialEscape : MonoBehaviour
{
    //Key to Transision
    private const KeyCode initiateTransisionButton = KeyCode.E;

    //Tag that is given to player characters
    private const string playerTag = "Player";

    //Scene to load for escape
    private const string sceneNameToLoad = "MainGame";


    /// <summary>
    /// When the player stays within the trigger allow them
    /// to exit the tutorial
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //Check that the player is trying to transision
        if (Input.GetKeyDown(initiateTransisionButton))
        {
            //Check that the collider is a character type
            if (other.gameObject.CompareTag(playerTag))
            {
                //Load Main Game Scene
                SceneManager.LoadScene(sceneNameToLoad);
            }
        }
    }
}