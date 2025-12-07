using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PaintMeshFusion : MonoBehaviour
{
    private Mesh paintMesh;

    public List<Vector3> vertices = new List<Vector3>();
    public List<Color> colors = new List<Color>();
    public List<int> triangles = new List<int>();

    private SpatialHashQuads spatialHash;
    public float spatialHashCellSize = 0.1f;

    public void Awake()
    {
        paintMesh = new Mesh();
        paintMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // support >65k vertices
        GetComponent<MeshFilter>().mesh = paintMesh;

        spatialHash = new SpatialHashQuads(spatialHashCellSize);
    }

    /// <summary>
    /// Paint or erase a stroke at position with given normal and color
    /// </summary>
    public bool Paint(Vector3 position, Vector3 normal, Color color, float size, bool erase = false)
    {
        float mergeSize = size / 2f;
        List<SpatialHashQuads.QuadData> nearby = spatialHash.GetNearbyQuads(position, mergeSize);
        
        bool needANewQuad = true;
        foreach (SpatialHashQuads.QuadData quad in nearby)
        {      
            if (Utils.MaxDistance(quad.center, position) <= mergeSize)
            {
                needANewQuad = false;
            }
        }

        // go through all vertices
        bool changedAColor = false;
        for(int i = 0; i < vertices.Count; i++)
        {
            
            if (Utils.MaxDistance(vertices[i], position) < size / 2.0f)
            {
                colors[i] = color;
                changedAColor = true;
            }
        }

        // If no nearby vertices, add a new quad
        if (needANewQuad && !erase)
        {
            Debug.Log("Added a quad");
            AddQuad(position, normal, color, size);
            return true;
        }

        if (changedAColor)
        {
            Debug.Log("Changed a color");
            paintMesh.SetColors(colors);
        }

        return false;
    }

    public bool Erase(Vector3 position, Vector3 normal, float size)
    {
        return Paint(position, normal, new Color(0, 0, 0, 0), size, true);
    }

    private void AddQuad(Vector3 position, Vector3 normal, Color color, float size)
    {
        Vector3 tangent = Vector3.Cross(normal, Vector3.up);
        if (tangent.sqrMagnitude < 0.001f) tangent = Vector3.Cross(normal, Vector3.right);
        tangent.Normalize();
        Vector3 bitangent = Vector3.Cross(normal, tangent);

        //offset the vertices by a little bit to avoid depth fighting
        float epsilon = 0.002f;
        Vector3 offSetPosition = position + normal * epsilon;

        float half = size * 0.5f;
        Vector3 v0 = offSetPosition + (tangent * half) + (bitangent * half);
        Vector3 v1 = offSetPosition + (tangent * half) - (bitangent * half);
        Vector3 v2 = offSetPosition - (tangent * half) - (bitangent * half);
        Vector3 v3 = offSetPosition - (tangent * half) + (bitangent * half);

        int index = vertices.Count;
        vertices.Add(v0); colors.Add(color);
        vertices.Add(v1); colors.Add(color);
        vertices.Add(v2); colors.Add(color);
        vertices.Add(v3); colors.Add(color);

        // Two triangles
        triangles.Add(index + 0); triangles.Add(index + 1); triangles.Add(index + 2);
        triangles.Add(index + 2); triangles.Add(index + 3); triangles.Add(index + 0);

        // Register quad in spatial hash
        Vector3 center = (v0 + v1 + v2 + v3) / 4f;
        spatialHash.AddQuad(center, index, index + 1, index + 2, index + 3);

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        paintMesh.SetVertices(vertices);
        paintMesh.SetColors(colors);
        paintMesh.SetTriangles(triangles, 0);
        paintMesh.RecalculateBounds();
    }
}
