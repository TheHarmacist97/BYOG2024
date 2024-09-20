using Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawingManager : MonoBehaviour
{
    [SerializeField]
    private PictureConfig[] _pictureConfigs;

    [SerializeField]
    private DrawingBase _drawingBase;

    [SerializeField]
    private float _drawingTime;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _descriptionText;

    [SerializeField]
    private TextMeshProUGUI _titleText;
    
    private int _maxDrawings = 5; 
    private int _currentDrawingIndex;

    public static DrawingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&_currentDrawingIndex<_maxDrawings)
        {
            StartDrawing();
        }
    }

    public void StartDrawing()
    {
        _descriptionText.SetText(_pictureConfigs[_currentDrawingIndex].description);;
        _titleText.SetText(_pictureConfigs[_currentDrawingIndex].pictureName);
        _drawingBase.StartNewDrawing();
        Debug.Log("Started New Drawing");
    }

    public void ValidateDrawing()
    {
        var drawTime = _drawingBase.GetDrawTime();
        if (drawTime < _drawingTime)
        {
            Debug.Log("Drawing is not finished");
            return;
        }
        PacmanConfig.SetDrawing(_pictureConfigs[_currentDrawingIndex].pictureID, _drawingBase.GetDrawing());
        if (_currentDrawingIndex < _maxDrawings - 1)
        {
            _currentDrawingIndex++;
        }
        else
        {
            SceneManager.LoadScene(3);
        }
        _drawingBase.StopDrawing();
    }
}