using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    MeshRenderer cubeMesh;
    Color currentColor;
    Color red = Color.red;
    Color transparentRed;

    void Awake()
    {
        cubeMesh = gameObject.GetComponent<MeshRenderer>();
        currentColor = cubeMesh.material.color;
        transparentRed = new Color(red.r, red.g, red.b, .4f);
    }

    void ApplyColorChange()
    {
        
        cubeMesh.material.color = Color.Lerp(currentColor, transparentRed, 1);
    }
}
