using System.Net.Sockets;
using Meta.WitAi;
using UnityEngine;


public class BrushPainter : MonoBehaviour
{
    public PaintMeshFusion paintSystem;
    public LayerMask worldMeshMask;
    public float brushSize = 0.1f;
    public Color brushColor = Color.red;
    public bool eraseMode = false;
    public float eraseSizeMultiplier = 2f;

    private static readonly int ColorProp = Shader.PropertyToID("_Color");
    public Material colorPreviewMat;
    public Material eraserMat;

    public MeshRenderer colorPreviewRenderer;

    public void Awake()
    {
        colorPreviewMat = new Material(colorPreviewMat);
        colorPreviewRenderer.material = colorPreviewMat;
        SetBrushColor(brushColor);
    }

    public void Update()
    {
        if(paintSystem == null)
        {
            paintSystem = GameObject.Find("PaintSystem")?.GetComponent<PaintMeshFusion>();
            if (paintSystem == null) return;
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.2f, worldMeshMask))
        {
            
            PaintingCanvas paintingCanvas = hit.collider.gameObject.GetComponent<PaintingCanvas>();
            if (paintingCanvas != null)
            {
                MeshCollider meshCollider = hit.collider.gameObject.GetComponent<MeshCollider>();

                Vector2 uv = hit.textureCoord;
                if (eraseMode) paintingCanvas.Paint(uv, new Color(1f, 1f, 1f, 1f));
                else paintingCanvas.Paint(uv, brushColor);
                
                return;
            }

            ColorPicker colorPicker = hit.collider.gameObject.GetComponent<ColorPicker>();
            if(colorPicker != null)
            {
                SetBrushColor(colorPicker.getColor(hit.point));
                SetErase(false);
            }

            EraserPicker eraserPicker = hit.collider.gameObject.GetComponent<EraserPicker>();
            if(eraserPicker != null)
            {
                SetErase(true);
            }


            if (hit.collider.gameObject.CompareTag("newOVR"))
            {
                if (eraseMode) paintSystem.Erase(hit.point, hit.normal, brushSize * eraseSizeMultiplier);
                else paintSystem.Paint(hit.point, hit.normal, brushColor, brushSize);
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
        Vector3 dir = transform.forward.normalized * 0.2f;

        Gizmos.DrawRay(start, dir);

        Vector3 right = Quaternion.AngleAxis(20, Vector3.up) * -dir * 0.2f;
        Vector3 left = Quaternion.AngleAxis(-20, Vector3.up) * -dir * 0.2f;

        Gizmos.DrawRay(start + dir, right);
        Gizmos.DrawRay(start + dir, left);
    }
}
