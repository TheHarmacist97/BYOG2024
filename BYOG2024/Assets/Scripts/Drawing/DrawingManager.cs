using System;
using Drawing;
using TMPro;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    [SerializeField]
    private PictureConfig[] _pictureConfigs;

    [SerializeField]
    private float[] _brushSizes;

    [SerializeField]
    private DrawingBase _drawingBase;

    [SerializeField]
    private float _drawingTime;

    [Header("UI")]
    [SerializeField]
    private GameObject _drawingUI;

    [SerializeField]
    private ButtonHoverBehaviour _clearButton;

    [SerializeField]
    private ButtonHoverBehaviour _validateButton;

    private int _maxDrawings = 5;
    private int _currentDrawingIndex;

    public static DrawingManager Instance;
    public event Action<int> DrawingCompleted;
    public event Action AllDrawingsCompleted;

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
        Debug.Log(_currentDrawingIndex);
        NotificationManager.Instance.SetNotification(_pictureConfigs[_currentDrawingIndex].description);
        _drawingBase.StartNewDrawing(_pictureConfigs[_currentDrawingIndex].canvasSize);
        _clearButton.SetInteractable(true);
        _validateButton.SetInteractable(true);
        Debug.Log("Started New Drawing");
    }

    public void ClearDrawing()
    {
        Debug.Log("Clearing Drawing");
        _drawingBase.StartNewDrawing();
    }

    public void SetBrushSize(int sizeIndex)
    {
        _drawingBase.SetBrushSize(_brushSizes[sizeIndex]);
    }

    public void ValidateDrawing()
    {
        var drawTime = _drawingBase.GetDrawTime();
        if (drawTime < _drawingTime)
        {
            Debug.Log("Drawing is not finished");
            return;
        }

        _drawingBase.StopDrawing();
        PacmanConfig.SetDrawing(_pictureConfigs[_currentDrawingIndex].pictureID, _drawingBase.GetDrawing());
        if (_currentDrawingIndex < _maxDrawings - 1)
        {
            _currentDrawingIndex++;
            DrawingCompleted?.Invoke(_currentDrawingIndex);
        }
        else
        {
            AllDrawingsCompleted?.Invoke();
        }
    }

    public void PauseDrawing()
    {
        //_drawingUI.SetActive(false);
        _clearButton.SetInteractable(false);
        _validateButton.SetInteractable(false);
        _drawingBase.PauseDrawing();
    }

    public void ResumeDrawing()
    {
        _drawingUI.SetActive(true);
        _drawingBase.ResumeDrawing();
    }
}