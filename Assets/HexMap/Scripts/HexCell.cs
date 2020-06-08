using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    // a list of the neighbors of the hex
    [SerializeField]
    HexCell[] neighbors = new HexCell[6];

    //the coordinates of the hex
    public HexCoordinates coordinates;
    //the color of the hex
    public Color color;

    //returns the neighbor in a given direction
    public HexCell getNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    //sets a neighbor
    public void setNeighbor(HexDirection direction, HexCell hex)
    {
        //store the hex as this ones neighbor
        neighbors[(int)direction] = hex;
        //the other hex this hex set as a neighbor in the opposite directions
        hex.neighbors[(int)direction.Opposite()] = this;
    }
}
