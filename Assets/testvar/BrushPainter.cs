using UnityEngine;

public class BrushPainter : MonoBehaviour
{
    public PaintMeshFusion paintSystem;
    public LayerMask worldMeshMask;
    public float brushSize = 0.1f;
    public Color brushColor = Color.red;
    public bool eraseMode = false;
    public float eraseSizeMultiplier = 2f;

    private Vector3 lastPaintPos;
    private Vector3 prevBrushPos;

    public void Awake()
    {
        prevBrushPos = transform.position;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!paintSystem) return;

        if (!other.gameObject.CompareTag("OVR")) return;

        if ((prevBrushPos - transform.position).magnitude < 0.01f * brushSize) return;
        prevBrushPos = transform.position;

        // Ray depuis le pinceau vers l’avant
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, worldMeshMask))
        {
            Vector3 pos = hit.point;

            bool placedAQuad;
            if (eraseMode) placedAQuad = paintSystem.Erase(pos, hit.normal, brushSize * eraseSizeMultiplier);
            else placedAQuad = paintSystem.Paint(pos, hit.normal, brushColor, brushSize);  
        }
    }

    public void SetBrushColor(Color c) => brushColor = c;
    public void SetBrushSize(float s) => brushSize = s;
    public void SetErase(bool enabled) => eraseMode = enabled;
}
