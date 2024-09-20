using Drawing;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    [SerializeField]
    private PictureConfig[] _pictureConfigs;

    [SerializeField]
    private DrawingBase _drawingBase;

    [SerializeField]
    private float _drawingTime;

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

    public void StartDrawing()
    {
        _drawingBase.StartNewDrawing();
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
    }
}