using UnityEngine;

public static class Utils
{
    public static float ManhattanDistance(Vector3 p1, Vector3 p2)
    {
        return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y) + Mathf.Abs(p1.z - p2.z);
    }

    public static float MaxDistance(Vector3 p1, Vector3 p2)
    {
        return Mathf.Max(Mathf.Abs(p1.x - p2.x), Mathf.Max(Mathf.Abs(p1.y - p2.y), Mathf.Abs(p1.z - p2.z)));
    }

}