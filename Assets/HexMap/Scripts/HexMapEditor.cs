using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

    //array of colors that can be used for the map
    public Color[] colors;

    //reference to the HexGrid
    public HexGrid hexGrid;

    //color that will actively be set
    private Color activeColor;

    void Awake()
    {
        SelectColor(0);
    }

    void Update()
    {
        //finds if the mouse is clicked
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    /// <summary>
    /// handles the input from the mouse
    /// </summary>
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            hexGrid.colorHex(hit.point, activeColor);
        }
    }

    /// <summary>
    /// sets the currently active color to the color at an index
    /// </summary>
    /// <param name="index"></param>
     public void SelectColor(int index)
    {
        activeColor = colors[index];
    }
}
