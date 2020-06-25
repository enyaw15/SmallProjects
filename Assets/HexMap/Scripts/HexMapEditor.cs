using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

    //array of colors that can be used for the map
    public Color[] colors;

    //reference to the HexGrid
    public HexGrid hexGrid;

    //the color that has been chosen
    private Color activeColor;
    //the elevation that has been chosen
    private int activeElevation;


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
            editHex(hexGrid.getHex(hit.point));
        }
    }

    private void editHex(HexCell hex)
    {
        hex.color = activeColor;
        hex.Elevation = activeElevation;
        hexGrid.refresh();
    }

    /// <summary>
    /// sets the currently active color to the color at an index
    /// </summary>
    /// <param name="index"></param>
    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    public void setElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }
}
