// ReSharper disable InconsistentNaming
using UnityEngine;

namespace CustomSaber;

public class CustomTrail : MonoBehaviour
{
    public Transform PointStart;
    public Transform PointEnd;
    public Material TrailMaterial;
    public ColorType colorType = ColorType.CustomColor;
    public Color TrailColor = Color.white;
    public Color MultiplierColor = Color.white;
    public int Length = 20;
}