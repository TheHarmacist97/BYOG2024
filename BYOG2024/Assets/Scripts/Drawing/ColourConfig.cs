using UnityEngine;

namespace Drawing
{
    [CreateAssetMenu(menuName = "Custom/ColourConfig", fileName = "ColourConfig", order = 0)]
    public class ColourConfig: ScriptableObject
    {
        [SerializeField]
        private Color[] _colour;
        public Color[] Colours => _colour;
    }
}