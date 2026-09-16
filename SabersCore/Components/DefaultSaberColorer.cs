using SaberComponents.Models;
using SabersCore.Utilities.Extensions;
using UnityEngine;

namespace SabersCore.Components;

internal class DefaultSaberColorer
{
    private readonly SetSaberGlowColor[] setSaberGlowColors;
    private readonly SetSaberFakeGlowColor[] setSaberFakeGlowColors;
    private readonly SaberType saberType;

    public DefaultSaberColorer(GameObject gameObject, SaberType saberType)
    {
        setSaberGlowColors = gameObject.GetComponentsInChildren<SetSaberGlowColor>();
        setSaberFakeGlowColors = gameObject.GetComponentsInChildren<SetSaberFakeGlowColor>();
        this.saberType = saberType;     
    }
    
    public void SetColor(ColorScheme colorScheme)
    {
        SetColor(saberType == SaberType.SaberA ? colorScheme.saberAColor :  colorScheme.saberBColor);
    }

    public void SetColor(Color color, SaberType saberType)
    {
        SetColor(color);
    }

    private void SetColor(Color color)
    {
        foreach (var setSaberGlowColor in setSaberGlowColors)
        {
            setSaberGlowColor.SetNewColor(color);
        }

        foreach (var setSaberFakeGlowColor in setSaberFakeGlowColors)
        {
            setSaberFakeGlowColor.SetNewColor(color);
        }
    }
}