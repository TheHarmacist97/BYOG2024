using UnityEngine;

[CreateAssetMenu(menuName = "Custom/PictureConfig", fileName = "Drawing")]
public class PictureConfig : ScriptableObject
{
    public string pictureID;

    public string pictureName;

    [TextArea]
    public string description;
}