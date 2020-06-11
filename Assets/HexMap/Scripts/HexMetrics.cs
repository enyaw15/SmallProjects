using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMetrics {

    //the outer radius of a hexagon
    public const float outerRadius = 10f;

    //the inner radius of a hexagon
    public const float innerRadius = outerRadius * 0.866025404f;//outerRadius * 3^1/2 /2

    //theses are used to blend colors only near the edge of the hexagons
    public const float solidFactor = .8f;//what fraction of the hex shoud be solid
    public const float blendFactor = 1f - solidFactor;//the fraction of the hex that is blended

    //holds the corners of a hexagon
    private static Vector3[] corners = {//switch x's and y's inorder to rotate hexagons by 90 degrees
        new Vector3(0f, 0f, outerRadius),                   // top
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),   // upper right
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),  // lower right
        new Vector3(0f, 0f, -outerRadius),                  // bottom
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius), // lower left
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),   // upper left
        new Vector3(0f, 0f, outerRadius)//one additional the same as the first to allow a complete loop (think of each corner after the first as being the end of a line this one would be needed to finish the hex
    };

    /// <summary>
    /// returns the first corner of an edge as specified by the direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns> returns the vector of the first corner of the hex in a given direction</returns>
    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int)direction];
    }


    /// <summary>
    /// returns the second corner of an edge as specified by the direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns> returns the vector of the second corner of the hex in a given direction</returns>
    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1];
    }

    //returns the position of the corner for the portion of the hexagon which is solid color
    public static Vector3 GetFirstSolidCorner (HexDirection direction) {
		return corners[(int)direction] * solidFactor;
	}

    //returns the position of the corner for the portion of the hexagon which is solid color
	public static Vector3 GetSecondSolidCorner (HexDirection direction) {
		return corners[(int)direction + 1] * solidFactor;
	}
    
}
