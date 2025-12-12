using System.Net.Sockets;
using Meta.WitAi;
using UnityEngine;
using UnityEngine.Rendering;

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
        if (!paintSystem)
        {
            paintSystem = GameObject.Find("PaintSystem")?.GetComponent<PaintMeshFusion>();
            if (!paintSystem) return;
        }

        if ((prevBrushPos - transform.position).magnitude < 0.01f * brushSize) return;
        prevBrushPos = transform.position;

        if (other.gameObject.CompareTag("OVR"))
        {
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


        PaintingCanvas paintingCanvas = other.gameObject.GetComponent<PaintingCanvas>();
        if (paintingCanvas != null)
        {
            MeshCollider meshCollider = other.gameObject.GetComponent<MeshCollider>();

            // Ray from the brush center toward the surface
            Ray ray = new Ray(transform.position, transform.forward);
            
            if (meshCollider.Raycast(ray, out RaycastHit hit, 1.0f))
            {
                Vector2 uv = hit.textureCoord;
                paintingCanvas.Paint(uv);
            }
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 start = transform.position;
        Vector3 dir = transform.forward.normalized;

        Gizmos.DrawRay(start, dir);

        Vector3 right = Quaternion.AngleAxis(20, Vector3.up) * -dir * 0.2f;
        Vector3 left = Quaternion.AngleAxis(-20, Vector3.up) * -dir * 0.2f;

        Gizmos.DrawRay(start + dir, right);
        Gizmos.DrawRay(start + dir, left);
    }
}
