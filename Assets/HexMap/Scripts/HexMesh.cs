using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the object must have a mesh filter and renderer
[RequireComponent(typeof (MeshFilter) ,typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

    //mesh of the hexes
    Mesh hexMesh;
    //collider of the mesh
    MeshCollider meshCollider;

    //holds the verticies of the Meshes
    List<Vector3> vertices;
    //List of the triangles in the mesh
    List<int> triangles;

    //list of all the colors in the mesh
    List<Color> colors;

    void Awake()
    {
        //create a new mesh and give a refference to the filter and to this script
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        hexMesh.name = "Hex Mesh";
        //add a collider 
        meshCollider = gameObject.AddComponent<MeshCollider>();
        //create lists
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    /// <summary>
    /// creates a mesh based on an array of HexCells (Triangulate)
    /// </summary>
    /// <param name="hexes"></param>
    public void createHexMesh(HexCell[] hexes)
    {
        //clear all the data so it can be recreated
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        //loop through and generate new triangles 
        for (int i = 0; i < hexes.Length; i++)
        {
            createHex(hexes[i]);
        }
        //set the vertices
        hexMesh.vertices = vertices.ToArray();
        //set the triangles
        hexMesh.triangles = triangles.ToArray();
        //calculate normals
        hexMesh.RecalculateNormals();
        //create a collider based on the mesh
        meshCollider.sharedMesh = hexMesh;
        //set the colors
        hexMesh.colors  = colors.ToArray();
    }

    /// <summary>
    /// creates the triangles that will represent a hex
    /// </summary>
    /// <param name="hex"></param>
    void createHex(HexCell hex)
    {   
        //go through all the neighbors of the hex to make edge tiles which colors can be blended over
        for(HexDirection d = HexDirection.UR; d<= HexDirection.UL; d++)
        {
            createHex(d, hex);
        }
    }
    
    /// <summary>
    /// create a triangle that will a part of the representation of a hex
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="hex"></param>
    void createHex(HexDirection direction, HexCell hex)
    {   
        //create six triangles to represent the hex(could be made four but that is more complicated)
        for (int i = 0; i < 6; i++)
        {
            //the center of the hex needed to find the corners
            Vector3 center = hex.transform.localPosition;
            //add a triangle to the triangles list
            AddTriangle(center, center + HexMetrics.GetFirstCorner(direction), center + HexMetrics.GetSecondCorner(direction));

            //get the neighbors of the hex that effect this triangle
            //get the previous neighbor
            HexCell prevNeighbor = hex.getNeighbor(direction.Previous()) ?? hex;
            //get the neighbor of the hex to get its color
            HexCell neighbor = hex.getNeighbor(direction) ?? hex;
            //get the next neighbor
            HexCell nextNeighbor = hex.getNeighbor(direction.Next()) ?? hex;
            //average the cell colors to get an accurate color of the blend
            Color edgeColor = (hex.color + neighbor.color) * 0.5f;
            //add the colors of the hex to the colors array (average the colors that effect the edge)
            AddTriangleColor(hex.color, (hex.color + prevNeighbor.color + neighbor.color) / 3f, (hex.color + neighbor.color + nextNeighbor.color) / 3f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

    /// <summary>
    /// creates a triangle from three vectors and adds the triangle to the verticies list and to the triangles list
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        //the point to begin adding the new corners to
        int vertexIndex = vertices.Count;
        //add three points which will be the trianlge's vertecies
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        //add three vertecies to make the triangle
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

}
