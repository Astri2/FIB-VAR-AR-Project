using UnityEngine;

public class PaintingCanvas : MonoBehaviour
{
    public int brushSize = 10;

    private Texture2D tex;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Create a white texture
        tex = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        ClearTexture();
        rend.material.mainTexture = tex;
    }

    public void Paint(Vector2 uv, Color paintColor)
    {
        int x = (int)(uv.x * tex.width);
        int y = (int)(uv.y * tex.height);

        // draw a simple square
        for (int i = -brushSize; i < brushSize; i++)
            for (int j = -brushSize; j < brushSize; j++)
                tex.SetPixel(x + i, y + j, paintColor);
        tex.Apply();
    }

    public void ClearTexture()
    {
        Color[] fill = new Color[tex.width * tex.height];
        for (int i = 0; i < fill.Length; i++) fill[i] = Color.white;
        tex.SetPixels(fill);
        tex.Apply();
    }
}
