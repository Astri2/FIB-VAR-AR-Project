using UnityEngine;

public class MyCollider : MonoBehaviour
{
    public GameObject kub; // prefab to instantiate

    private void Start()
    {
        // Get the OVRSceneAnchor (optional, for clarity)
        OVRSceneAnchor anchor = GetComponent<OVRSceneAnchor>();
        if (anchor == null)
        {
            Debug.LogWarning("No OVRSceneAnchor found!");
        }

        // Get the MeshFilter (the generated mesh of the plane or cube)
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>(false);
        if (meshFilter == null)
        {
            Debug.LogWarning("No MeshFilter found!");
            return;
        }

        // Instantiate kub
        GameObject instance = Instantiate(kub);

        // Match position, rotation, and scale to the mesh
        instance.transform.position = meshFilter.transform.position;
        instance.transform.rotation = meshFilter.transform.rotation;

        // Scale the kub to match the mesh bounds in world space
        Vector3 meshSize = meshFilter.mesh.bounds.size;
        Vector3 meshScale = meshFilter.transform.lossyScale;
        instance.transform.localScale = new Vector3(
            meshSize.x * meshScale.x,
            meshSize.y * meshScale.y,
            meshSize.z * meshScale.z
        );
    }
}
