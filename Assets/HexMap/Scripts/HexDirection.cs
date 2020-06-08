using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexDirection
{
    /*Definition
     * U stands for UP
     * D stands for Down
     * R stands for Right
     * L stands for Left
     */
     UR, R, DR, DL, L, UL
   //NE, E, SE, SW, W, NW
}

//returns the opposite direction of the current one
public static class HexDirectionExtensions
{

    /// <summary>
    /// returns the opposite direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns> returns the opposite direction </returns>
    public static HexDirection Opposite(this HexDirection direction)
    {
        //if it is less than three add three to get the opposite if it is greater than three subtract three to get the opposite
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    /// <summary>
    /// returns the previous direction without going out of bounds
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>the previous direction</returns>
    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.UR ? HexDirection.UL : (direction - 1);
    }

    /// <summary>
    /// returns the next direction without going out of bounds
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>the next direction</returns>
    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.UL ? HexDirection.UR : (direction + 1);
    }
}


