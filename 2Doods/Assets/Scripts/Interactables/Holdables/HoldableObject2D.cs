using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject2D : HoldableObject
{
    //3D Object that this object converts in to when moving
    //3D -> 2D
    [SerializeField]
    private GameObject convertObject;
    //Property for the convert object, only return the game object if 
    //it is a valid 3D Holdable Object
    public GameObject ConvertObject
    {
        get
        {
            if (convertObject.GetComponent<HoldableObject3D>())
            {
                return convertObject;
            }
            else
            {
                return null;
            }
        }
    }
}
