using UnityEngine;

public enum PictureIDs
{
    Pacman,
    Ghost1,
    Ghost2,
    Ghost3,
    Food
}

[CreateAssetMenu(menuName = "Custom/PictureConfig", fileName = "Drawing")]
public class PictureConfig : ScriptableObject
{
    public PictureIDs pictureID;

    public string pictureName;

    [TextArea]
    public string description;

    public Vector2 canvasSize;
}