using UnityEngine;

public class BrushPainter : MonoBehaviour
{
    public PaintMeshFusion paintSystem;
    public LayerMask worldMeshMask;
    public float brushSize = 0.1f;
    public Color brushColor = Color.red;
    public bool eraseMode = false;
    public float eraseSizeMultiplier = 2f;

    private Vector3 prevBrushPos;

    private static readonly int ColorProp = Shader.PropertyToID("_Color");
    public Material colorPreviewMat;
    public Material eraserMat;

    public MeshRenderer colorPreviewRenderer;

    public void Awake()
    {
        prevBrushPos = transform.position;
        SetBrushColor(brushColor);
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

            if (eraseMode) paintSystem.Erase(pos, hit.normal, brushSize * eraseSizeMultiplier);
            else paintSystem.Paint(pos, hit.normal, brushColor, brushSize);  
        }
    }

    public void SetBrushColor(Color c)
    {
        brushColor = c;
        colorPreviewMat.SetColor(ColorProp, c);
    }
    public void SetBrushSize(float s) => brushSize = s;
    public void SetErase(bool enabled)
    {
        if(!enabled && eraseMode)
        {
            colorPreviewRenderer.material = colorPreviewMat;
        } else if(enabled && !eraseMode)
        {
            colorPreviewRenderer.material = eraserMat;
        }
        eraseMode = enabled;
    }
}
