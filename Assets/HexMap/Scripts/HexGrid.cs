using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    //hold color
    public Color defaultColor = Color.white;

    //height and width of the grid
    public int height = 10;
    public int width = 10;

    //prefab of a cell
    public HexCell cellPrefab;

    //the grid of HEXES
    private HexCell[] hexGrid;

    //label for the hexes so that their coordinates can be displayed
    public Text HexLabelPrefab;

    //canvas for displaying hex labels
    private Canvas gridCanvas;

    //holds the mesh of this grid
    HexMesh hexMesh;

    void Awake()
    {
        //get a reference to hexmesh
        hexMesh = GetComponentInChildren<HexMesh>();
        //get a reference to the canvas
        gridCanvas = GetComponentInChildren<Canvas>();

        //create the array of hexes
        hexGrid = new HexCell[height * width];

        //loop through hexes and create them the x and z represennt the positions of the hexes arranged in a rectagle rather than the parallelogram that they exist in
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateHex(x,z, i++);
            }
        }
    }

    void Start()
    {
        //tell the hexmesh to make triangles for the hexes
        hexMesh.createHexMesh(hexGrid);
    }


    /// <summary>
    /// create a hex at a given position xz with an index of i in the hexgrid array
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="i"></param>
    private void CreateHex(int x, int z, int i)
    {
        //the position of the cell is created
        Vector3 position;
        //uses truncation to get the offset for odd numbers it adds .5 since truncation causes the z/2 to round down while the .5f stays accurate
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * HexMetrics.outerRadius*1.5f;

        //creats a new cell which is moved to the position
        HexCell hex = hexGrid[i] = Instantiate(cellPrefab);
        hex.transform.SetParent(transform, false);
        hex.transform.localPosition = position;
        hex.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        
        //labels the cell with its coordinates coordinate
        Text label = Instantiate(HexLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = hex.coordinates.ToStringOnSeparateLines();

        //assign the hex a color
        hex.color = defaultColor;

        //assign neighbors(expicitly neighbors below and to the left, implicitly neighbors above and to the right)
        if (x > 0)//all hexes with x > 0 have a neighbor to the Left
        {
            //assign a neightbor to the Left
            hex.setNeighbor(HexDirection.L, hexGrid[i - 1]);
        }
        if (z > 0)//all hexes with a z>0 have atleast one neighbor below
        {
            if ((z % 2) == 0)//split even and odd row hex logic
            {
                //this row has a right neighbor on the end since it does not stick out on the right side
                hex.setNeighbor(HexDirection.DR, hexGrid[i - width]);
                if (x > 0)//has a left Down neighbor
                {
                    hex.setNeighbor(HexDirection.DL, hexGrid[i - width - 1]);
                }
            }
            else
            {
                //this row has a left neighbor on the end since it does not stick out on the left side
                hex.setNeighbor(HexDirection.DL, hexGrid[i - width]);
                if (x < width - 1)//the hexes on the right edge do not have right neigbors
                {
                    hex.setNeighbor(HexDirection.DR, hexGrid[i - width + 1]);
                }
            }
        }
    }

    /*
     *  returns the hex at a given position 
     */
    public HexCell getHex(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        return hexGrid[index];
    }

    /*
     *  remakes the mesh
     */
     public void refresh()
     {
         hexMesh.createHexMesh(hexGrid);
     }
}
