using System;
using System.Collections.Generic;
using UnityEngine;

public static class PacmanConfig
{
    public static readonly Dictionary<string, Sprite> Drawings = new();

    public static void SetDrawing(string pictureID, RenderTexture renderTexture)
    {
        Debug.Log($"Saving to {pictureID}");
        Drawings[pictureID] = renderTexture.ToTexture2D();
    }

    public static Sprite ToTexture2D(this RenderTexture rTex)
    {
        if (rTex == null)
        {
            Debug.LogError("RenderTexture is null.");
            return null;
        }

        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point
        };
        var oldRT = RenderTexture.active;
        RenderTexture.active = rTex;

        try
        {
            Graphics.ConvertTexture(rTex, tex);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading pixels from RenderTexture: {e.Message}");
            return null;
        }
        finally
        {
            RenderTexture.active = oldRT;
        }
    }
}