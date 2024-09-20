using System;
using UnityEngine;
using UnityEngine.UI;

namespace Drawing
{
    public class ColourPalette : MonoBehaviour
    {
        [SerializeField]
        private ColourConfig _colourConfig;

        [SerializeField]
        private Button _colourConfigPrefab;
        
        public event Action<Color> OnColourChanged;
        
        private Color _activeColour;
        public Color ActiveColour
        {
            get
            {
                return _activeColour;
            }
            private set
            {
                _activeColour = value;
                OnColourChanged?.Invoke(_activeColour);
            }
        }

        private void Start()
        {
            for (var i = 0; i < _colourConfig.Colours.Length; i++)
            {
                var colour = _colourConfig.Colours[i];
                var button = Instantiate(_colourConfigPrefab, transform);
                button.GetComponent<Image>().color = colour;
                var i1 = i;
                button.onClick.AddListener(() => SetColour(i1));
            }

            ActiveColour = _colourConfig.Colours[0];
        }

        private void OnDestroy()
        {
            OnColourChanged = null;
        }

        private void SetColour(int index)
        {
            ActiveColour = _colourConfig.Colours[index];
        }
    }
}