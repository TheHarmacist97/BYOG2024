using UnityEngine;

namespace Drawing
{
    public class ColourPalette : MonoBehaviour
    {
        [SerializeField]
        private ColourConfig _colourConfig;

        private Color _activeColour;

        public void SetColour(int index)
        {
            _activeColour = _colourConfig.Colours[index];
        }
    }
}