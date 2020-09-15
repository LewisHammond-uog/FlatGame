using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvanceToTutorial : MonoBehaviour
{

    //Name of tutorial level to load
    private const string tutorialLevelName = "Tutorial"; 

    /// <summary>
    /// Load the tutorial level.
    /// Called by animator event
    /// </summary>
    public void LoadTutorial()
    {
        SceneManager.LoadScene(tutorialLevelName);
    }
}
