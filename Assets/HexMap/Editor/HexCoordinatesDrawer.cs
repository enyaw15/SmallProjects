using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//this is a custom drawer for just HexCoordinates
[CustomPropertyDrawer(typeof(HexCoordinates))]
//child of property drawer
public class HexCoordinatesDrawer : PropertyDrawer{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //retrieve the coordinates 
        HexCoordinates coordinates = new HexCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
        );

        // label the coordinates 
        position = EditorGUI.PrefixLabel(position, label);
        
        //change the label
        GUI.Label(position, coordinates.ToString());
    }

}
