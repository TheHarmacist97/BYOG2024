using Unity.Mathematics;
using UnityEngine;

namespace Drawing
{
    public class DrawingBase : MonoBehaviour
    {
        [SerializeField]
        private ColourPalette _colourPalette;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private Transform _backgroundTransform;

        [SerializeField]
        private int2 _drawingScale = new int2(32, 64);

        [SerializeField]
        private Vector2 _drawingSize = new Vector2(5f, 7.5f);

        private int2 _currentDrawingSize;
        private CustomRenderTexture _customRenderTexture;
        private Renderer _renderer;
        private Material _drawingMaterial;

        private static readonly int RenderTexture = Shader.PropertyToID("_Texture");
        private static readonly int BrushColor = Shader.PropertyToID("_BrushColor");
        private static readonly int BrushCoord = Shader.PropertyToID("_BrushCoord");
        private static readonly int BrushSize = Shader.PropertyToID("_BrushSize");

        private RaycastHit[] _raycastHits;
        private bool _isDrawing;
        private float _timer;


        private void Start()
        {
            CacheVariables();
            _colourPalette.OnColourChanged += OnColourChanged;
        }

        private void CacheVariables()
        {
            _renderer ??= GetComponent<Renderer>();
            _drawingMaterial ??= _renderer.material;
            _raycastHits ??= new RaycastHit[1];
        }

        private void OnDestroy()
        {
            _colourPalette.OnColourChanged -= OnColourChanged;
        }

        private void Update()
        {
            if (!_isDrawing) return;
            Draw();
        }

        private void Draw()
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (!(Physics.RaycastNonAlloc(ray, _raycastHits, 100f, _layerMask,
                    QueryTriggerInteraction.Ignore) > 0)) return;
            var uv = _raycastHits[0].textureCoord;
            if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1)
                return;

            if (Input.GetMouseButton(0))
                _customRenderTexture.Initialize();
            _drawingMaterial.SetVector(BrushCoord, new Vector4(uv.x, uv.y, 0, 0));
            if (Input.GetMouseButton(0))
            {
                _customRenderTexture.Update();
                _timer += Time.deltaTime;
            }
        }

        public void PauseDrawing()
        {
            _isDrawing = false;
        }

        public void ResumeDrawing()
        {
            _isDrawing = true;
        }
        
        public void StopDrawing()
        {
            _isDrawing = false;
            _timer = 0f;
        }

        public void SetBrushSize(float size)
        {
            _drawingMaterial.SetFloat(BrushSize, size);
        }

        public void StartNewDrawing(int2 size = default)
        {
            _timer = 0;
            _isDrawing = true;
            if (!_drawingMaterial)
                CacheVariables();
            if (size.Equals(default))
            {
                size = _currentDrawingSize;
            }
            else
            {
                _currentDrawingSize = size;
                UpdateScale(_currentDrawingSize);
            }

            _customRenderTexture =
                new CustomRenderTexture(size.x, size.y, RenderTextureFormat.ARGBInt, RenderTextureReadWrite.Linear)
                {
                    filterMode = FilterMode.Point,
                    updateMode = CustomRenderTextureUpdateMode.OnDemand,
                    initializationSource = CustomRenderTextureInitializationSource.Material,
                    initializationMaterial = _drawingMaterial,
                    doubleBuffered = true
                };

            _drawingMaterial.SetTexture(RenderTexture, _customRenderTexture);
            _drawingMaterial.SetColor(BrushColor, _colourPalette.ActiveColour);
        }

        private void UpdateScale(int2 currentSize)
        {
            var scaleX = Mathf.InverseLerp(_drawingScale.x, _drawingScale.y, currentSize.x);
            var scaleY = Mathf.InverseLerp(_drawingScale.x, _drawingScale.y, currentSize.y);
            scaleX = Mathf.Lerp(_drawingSize.x, _drawingSize.y, scaleX);
            scaleY = Mathf.Lerp(_drawingSize.x, _drawingSize.y, scaleY);
            var newSize = new Vector3(scaleX, scaleY, 1f);
            transform.localScale = newSize;
            _backgroundTransform.localScale = newSize;
        }

        public RenderTexture GetDrawing()
        {
            return _customRenderTexture;
        }

        public float GetDrawTime()
        {
            Debug.Log("Drawing time: " + _timer);
            return _timer;
        }

        private void OnColourChanged(Color colour)
        {
            Debug.Log("Colour changed to: " + colour);
            _drawingMaterial.SetColor(BrushColor, colour);
        }
    }
}