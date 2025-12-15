using Meta.WitAi;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public Color getColor(Vector3 hitPosition)
    {
        Vector3 localHitPosition = transform.InverseTransformPoint(hitPosition);
        Vector2 hit = new Vector2(localHitPosition.x, localHitPosition.z);

       
        float v = 1.0f;
        float s = Mathf.Min(hit.magnitude / 0.5f, 1f); // outside ring of the circle is at 0.5 of center

        float h = Mathf.Atan2(hit.y, hit.x);
        // rescale between 0 and 1
        float oldH = h;
        float twoPi = 2f * Mathf.PI;
        h = (h % twoPi) + (h < 0 ? twoPi : 0f); // C# mod is dumb.
        h /=  twoPi;

        // Convert HSV to RGB
        // https://stackoverflow.com/questions/51203917/math-behind-hsv-to-rgb-conversion-of-colors
        float i = Mathf.Floor(6 * h);
        float f = 6 * h - i;
        float p = v * (1 - s);
        float q = v * (1 - f * s);
        float t = v * (1 - (1 - f) * s);

        float r, g, b;
        switch (i % 6)
        {
            case 0: r = v; g = t; b = p; break;
            case 1: r = q; g = v; b = p; break;
            case 2: r = p; g = v; b = t; break;
            case 3: r = p; g = q; b = v; break;
            case 4: r = t; g = p; b = v; break;
            default: r = v; g = p; b = q; break;
        }

        return new Color(r, g, b, 1.0f);
    }

}
