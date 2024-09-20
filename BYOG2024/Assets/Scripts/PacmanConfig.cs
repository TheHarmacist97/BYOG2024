using System.Collections.Generic;
using UnityEngine;

public static class PacmanConfig
{
    public static readonly Dictionary<string, RenderTexture> Drawings = new();
    
    public static void SetDrawing(string pictureID, RenderTexture renderTexture)
    {
        Drawings[pictureID] = renderTexture;
    }
}