using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlighting : MonoBehaviour
{
    //Shader Loading
    private const string shaderLoadPath = "Shaders/CustomHighlight";
    private static Shader holdableHighlightShader;

    //Colour to hightlight the object in when being looked at
    private readonly Color highlightColor = Color.white;
    private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

    protected void Awake()
    {
        //Load Shader from resources
        if (holdableHighlightShader == null)
        {
            holdableHighlightShader = LoadShader();
        }

        //Get out current materials texture
        Texture currentTexture = GetTexture();

        //Create a new material with our holdable shader and give it our current texutre
        Material newMaterial = new Material(holdableHighlightShader) {mainTexture = currentTexture};

        //Apply the new created material
        SetMaterial(newMaterial);

        //Set outline colour
        gameObject.GetComponent<Renderer>().sharedMaterial.SetColor(OutlineColor, highlightColor);
    }

    /// <summary>
    /// Sets if there is an outline drawn around this objet
    /// </summary>
    /// <param name="draw">If the outline should be drawn</param>
    public void SetDrawOutline(bool draw)
    {
        if (draw)
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.SetInt("_DrawOutline", 1);
        }
        else
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.SetInt("_DrawOutline", 0);
        }
    }

    /// <summary>
    /// Load shader from file
    /// </summary>
    /// <returns></returns>
    private Shader LoadShader()
    {
        return Resources.Load<Shader>(shaderLoadPath);
    }


    /// <summary>
    /// Get the current texture of the holdable object
    /// </summary>
    /// <returns>Current Texture</returns>
    protected Texture GetTexture()
    {
        return GetComponent<MeshRenderer>()?.material.mainTexture;
    }

    /// <summary>
    /// Set the current material of the holdable object
    /// </summary>
    /// <param name="newMat"></param>
    protected void SetMaterial(Material newMat)
    {
        GetComponent<MeshRenderer>().material = newMat;
    }
}
