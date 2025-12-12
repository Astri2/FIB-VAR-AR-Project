using UnityEngine;

public class PaintingCanvas : MonoBehaviour
{
    public Camera cam;           // Ta caméra principale
    public Color paintColor = Color.red;
    public int brushSize = 10;

    private Texture2D tex;
    private Renderer rend;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        rend = GetComponent<Renderer>();

        // Crée une texture blanche
        tex = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        Color[] fill = new Color[tex.width * tex.height];
        for (int i = 0; i < fill.Length; i++) fill[i] = Color.white;
        tex.SetPixels(fill);
        tex.Apply();

        // Assigne la texture au matériau
        rend.material.mainTexture = tex;
    }
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Vector2 uv = hit.textureCoord;
                    Paint(uv);
                }
            }
        }
    }

    public void Paint(Vector2 uv)
    {
        int x = (int)(uv.x * tex.width);
        int y = (int)(uv.y * tex.height);

        // dessine un carré simple comme pinceau
        for (int i = -brushSize; i < brushSize; i++)
            for (int j = -brushSize; j < brushSize; j++)
                tex.SetPixel(x + i, y + j, paintColor);
        tex.Apply();
    }
}
