using UnityEngine;

namespace SabersCore.Utilities.Extensions;

internal static class MathExtensions
{
    public static bool Approximately(this float a, float b) => Mathf.Approximately(a, b);
}