using System;
using System.Collections.Generic;
using UnityEngine;

public static class PacmanConfig
{
    public static readonly Dictionary<PictureIDs, Sprite> Drawings = new();

    public static string PacmanName { get; set; } = "Pacman";
    public static float OverallSuccessRate { get; set; } = 1f;

    public static float ProgrammingSuccess { get; set; } = 1f;
    public static float SoundSuccess { get; set; } = 1f;

    public static void SetDrawing(PictureIDs pictureID, RenderTexture renderTexture)
    {
        Debug.Log($"Saving to {pictureID}");
        Sprite sprite = renderTexture.ToTexture2D();
        Debug.Log(sprite.pivot);
        Drawings.Add(pictureID, sprite);
    }

    /// <summary>
    /// Call this to reset all the shit in the Config
    /// </summary>
    public static void PurgeConfig()
    {
        Drawings.Clear();
        OverallSuccessRate = 0f;
        ProgrammingSuccess = 0f;
        SoundSuccess = 0f;
    }

    private static Sprite ToTexture2D(this RenderTexture rTex)
    {
        if (rTex == null)
        {
            Debug.LogError("RenderTexture is null.");
            return null;
        }

        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
        };
        var oldRT = RenderTexture.active;
        RenderTexture.active = rTex;

        try
        {
            Graphics.ConvertTexture(rTex, tex);
            return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), 
                Vector2.one*0.5f, rTex.width, 0, SpriteMeshType.Tight);
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