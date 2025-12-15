using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserPicker : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        BrushPainter brush = other.GetComponent<BrushPainter>();
        if (brush == null) return;

        brush.SetErase(true);
    }
}
