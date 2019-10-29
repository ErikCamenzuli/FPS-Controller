using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGen)),CanEditMultipleObjects]
public class MapGenEdit : Editor
{
    public override void OnInspectorGUI()
    {
        //getting reference to the map generator and casting 
        //the target to the map generator then drawing the inspector
        MapGen mapGen = (MapGen)target;

        //checking to see if any value was changed
        if(DrawDefaultInspector())
        {
            //if there is an update then it will automatically update
            //the map generator
            if(mapGen.autoUpdate)
            {
                mapGen.DrawingInEditor();
            }
        }

        //if the button is pressed, it will generate a map
        if(GUILayout.Button ("Generate"))
        {
            mapGen.DrawingInEditor();
        }
    }
}
