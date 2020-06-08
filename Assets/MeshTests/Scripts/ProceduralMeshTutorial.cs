using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]//requires that the object has these components
public class ProceduralMeshTutorial : MonoBehaviour {

	
    public int xSize, ySize;
    private Mesh mesh;//holds a procedural mesh
    private Vector3[] vertices;//holds the verticies of a mesh in 3d space

    private void Awake()//starts on play
    {
        Generate();//calls generate
    }

    private void Generate()//generates mesh
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();//get reference to the mesh and make it a new mesh
        mesh.name = "Procedural Grid";//name the mesh

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];//create vertices array to hold 3d coordinates
        Vector2[] uv = new Vector2[vertices.Length];//holds the uv coordinates for the mesh
        Vector4[] tangents = new Vector4[vertices.Length];//holds the tangents of the mesh
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);// creates a tangent for the mesh
        for (int i = 0, y = 0; y <= ySize; y++)//creates all coordinates
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);//creates a vertex
                uv[i] = new Vector2(x / (float)xSize, y / (float)ySize);//creates uvs for the vertex
                tangents[i] = tangent;//creates tangent for the vertex
            }
        }
        mesh.vertices = vertices;//assign the vertices to the mesh
        mesh.uv = uv;//assigns uv coordinates
        mesh.tangents = tangents;//assigns tangents

        int[] triangles = new int[xSize * ySize * 6];//triangles to be used in mesh rendering for each unit their will be six triangle coordinates two are shared
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)//ti index in the triangles array // offset so the triangles move to the next square// x and y track the progress of the loop
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;
		mesh.RecalculateNormals();
    }
}
