using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    //allows the coordinates to be seen in the editor
    [SerializeField]
    private int x, z;

    //holds the x value
    public int X
    {
        get
        { return x; }
        private set
        { x = value; }
    }

    //holds the y value
    public int Z
    {
        get
        { return z; }
        private set
        { z = value; }
    }
    //returns the correct y value based on the XZ coordinates
    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    /*
     * accepts the coordinates of the hex in the hexgrid
     */
    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    /*
     * returns the correct coordinates for a hex based on the position of the hexes in an array
     */
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x -z/2, z);
    }

    /*
     * returns the correct coordinates for a hex based on the position in space
     */
    public static HexCoordinates FromPosition(Vector3 position)
    {
        //use the x of the vector to find out which hex it is in
        float x = position.x / (HexMetrics.innerRadius * 2f);
        //the y is the negative x
        float y = -x;

        //find the offset due to z
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        //apply the offset to the values
        x -= offset;
        y -= offset;

        //round all the numbers to the nearest whole
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        //the coordinates must always equal 0 the rounding in the previous step can introduce errors in edge cases due to the shape of hexagons
        if (iX + iY + iZ != 0)
        {
            //if they dont add up find the error introduced in rounding
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            //this block of ifs remakes the coordinate with the highest error based off of the other coordinates(Y is not checked since we dont need it to make a new HexCoordinate
            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        //create and return a new hexcoordinate
        return new HexCoordinates(iX, iZ);
    }

    /*
     * returns a string representaion
     */
    public override string ToString()
    {
        return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
    }

    /*
     * returns a string representaion on multiple lines
     */
    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}