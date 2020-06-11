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

    private void Awake()
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
    /// <param name="hexes">the array of hexes</param>
    public void createHexMesh(HexCell[] hexes)
    {
        //clear all the data so it can be recreated
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        //loop through and generate new triangles for each hex
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
    private void createHex(HexDirection direction, HexCell hex)
    {   
        //create six triangles to represent the hex(could be made four but that is more complicated)
            //the center of the hex needed to find the corners
            Vector3 center = hex.transform.localPosition;
            //holds the value of the corners of the solid part of the hexagon
		    Vector3 v1 = center + HexMetrics.getFirstSolidCorner(direction);
		    Vector3 v2 = center + HexMetrics.getSecondSolidCorner(direction);
            //add a solid color triangle to the triangles list
            AddTriangle(center, v1, v2);
            AddTriangleColor(hex.color);            

            //create the area of the hex that is color blended
            createBlendedHex(direction, hex,v1,v2);

            /*
            //get the neighbors of the hex
            //get the previous neighbor
            HexCell prevNeighbor = hex.getNeighbor(direction.Previous()) ?? hex;
            //get the neighbor of the hex to get its color
            HexCell neighbor = hex.getNeighbor(direction) ?? hex;
            //get the next neighbor
            HexCell nextNeighbor = hex.getNeighbor(direction.Next()) ?? hex;
            //average the hex colors to get an accurate color of the blend
            Color edgeColor = (hex.color + neighbor.color) * 0.5f;
            //add the blended color quad to the mesh
            AddQuad(v1,v2,v3,v4);
            AddQuadColor(hex.color, (hex.color + neighbor.color) / 2f);

            //create the triangles for the 3 way intersection of hexes
            AddTriangle(v1, center + HexMetrics.getFirstCorner(direction), v3);
		    AddTriangleColor(hex.color, (hex.color + prevNeighbor.color + neighbor.color) / 3f, edgeColor);
            AddTriangle(v2, v4, center + HexMetrics.getSecondCorner(direction));
		    AddTriangleColor(hex.color, edgeColor, (hex.color + neighbor.color + nextNeighbor.color) / 3f);
            */
    }

    private void createBlendedHex(HexDirection direction, HexCell hex, Vector3 v1, Vector3 v2)
    {
        //get the neighbor of the hex
        HexCell neighbor = hex.getNeighbor(direction);
		if (neighbor == null) {
			return;
		}
        
        //get the outer corners of the quad
        Vector3 quadOffset = HexMetrics.getBlendQuad(direction);//the offset from the original corners
		Vector3 v3 = v1 + quadOffset;
		Vector3 v4 = v2 + quadOffset;

        if(direction <= HexDirection.DR)
        {
            //add the blended color quad to the mesh
            AddQuad(v1,v2,v3,v4);
            AddQuadColor(hex.color, neighbor.color);
        }
        //create a blended triangle 
        HexCell nextNeighbor = hex.getNeighbor(direction.Next());
		if (direction <= HexDirection.R && nextNeighbor != null) {
            AddTriangle(v2, v4, v2 + HexMetrics.getBlendQuad(direction.Next()));
			AddTriangleColor(hex.color, neighbor.color, nextNeighbor.color);
		}
    }

    /// <summary>
    /// adds the colors of a blended triangle
    /// </summary>
    /// <param name="c1">the first color</param>
    /// <param name="c2">the second color</param>
    /// <param name="c3">the third color</param>
    private void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

    /// <summary>
    /// adds the color of a solid triangle
    /// </summary>
    /// <param name="c1">the color of the triangle</param>
    private void AddTriangleColor(Color c1)
    {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c1);
    }

    /// <summary>
    /// creates a triangle from three vectors and adds the triangle to the verticies list and to the triangles list
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
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

    //creates a quad 
    //this is better than adding two triangles because it makes the vertices list smaller
    //this is used to create color blended regions between the hexes
    private void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
		int vertexIndex = vertices.Count;
        //add the vertices of the quad
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		vertices.Add(v4);
        //create the two triangles of the quad
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

    /*
    //adds the colors to the quad
	private void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
		colors.Add(c4);
	}
    */
    //takes two colors to color a quad
    private void AddQuadColor (Color c1, Color c2) {
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c2);
	}
}
