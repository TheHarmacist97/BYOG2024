using UnityEngine;

namespace Drawing
{
    public class DrawingBase : MonoBehaviour
    {
        [SerializeField]
        private ColourPalette _colourPalette;

        [SerializeField]
        private LayerMask _layerMask;

        private Material _drawingMaterial;

        private CustomRenderTexture _customRenderTexture;
        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _drawingMaterial = _renderer.material;
            _customRenderTexture =
                new CustomRenderTexture(32, 32, RenderTextureFormat.ARGBInt, RenderTextureReadWrite.Linear);
            _customRenderTexture.filterMode = FilterMode.Point;
            _customRenderTexture.updateMode = CustomRenderTextureUpdateMode.OnDemand;
            _customRenderTexture.initializationSource = CustomRenderTextureInitializationSource.Material;
            _customRenderTexture.initializationMaterial = _drawingMaterial;
            _customRenderTexture.doubleBuffered = true;
            _drawingMaterial.SetTexture("_Texture", _customRenderTexture);
            _drawingMaterial.SetColor("_BrushColor", Color.red);
        }

        private void Update()
        {
            //Draw
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1f, _layerMask,
                    QueryTriggerInteraction.Ignore))
            {
                var uv = hit.textureCoord;
                if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1)
                    return;

                if (Input.GetMouseButton(0))
                    _customRenderTexture.Initialize();
                Debug.Log("Rendering at: " + uv);
                _drawingMaterial.SetVector("_BrushCoord", new Vector4(uv.x, uv.y, 0, 0));
                if (Input.GetMouseButton(0))
                    _customRenderTexture.Update();
            }
        }
    }
}