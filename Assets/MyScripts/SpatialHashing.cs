using System.Collections.Generic;
using UnityEngine;


public class SpatialHashQuads
{
    private float cellSize;
    private Dictionary<Vector3Int, List<QuadData>> hash = new Dictionary<Vector3Int, List<QuadData>>();

    public SpatialHashQuads(float cellSize)
    {
        this.cellSize = cellSize;
    }

    /// <summary>
    /// Represents a quad in the mesh
    /// </summary>
    public class QuadData
    {
        public Vector3 center;
        public List<int> indices;
    }

    private Vector3Int PositionToCell(Vector3 pos)
    {
        return new Vector3Int(
            Mathf.FloorToInt(pos.x / cellSize),
            Mathf.FloorToInt(pos.y / cellSize),
            Mathf.FloorToInt(pos.z / cellSize)
        );
    }

    /// <summary>
    /// Add a new quad to the hash
    /// </summary>
    public void AddQuad(Vector3 center, int v0, int v1, int v2, int v3, int v4)
    {
        Vector3Int cell = PositionToCell(center);
        if (!hash.TryGetValue(cell, out var list))
        {
            list = new List<QuadData>();
            hash[cell] = list;
        }

        QuadData qd = new();
        qd.center = center;
        qd.indices = new List<int> { v0, v1, v2, v3, v4 };
        list.Add(qd);
    }

    /// <summary>
    /// Returns all quads whose centers are within a given radius of position
    /// </summary>
    public List<QuadData> GetNearbyQuads(Vector3 position, float radius)
    {
        int offset = Mathf.CeilToInt(radius / cellSize);
        Vector3Int centerCell = PositionToCell(position);
        List<QuadData> result = new List<QuadData>();

        for (int x = -offset; x <= offset; x++)
        for (int y = -offset; y <= offset; y++)
        for (int z = -offset; z <= offset; z++)
        {
            Vector3Int neighbor = centerCell + new Vector3Int(x, y, z);
            if (hash.TryGetValue(neighbor, out var quads))
            {
                result.AddRange(quads);
            }
        }

        return result;
    }

    public void Clear()
    {
        hash.Clear();
    }
}
