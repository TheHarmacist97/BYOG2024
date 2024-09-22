using System;
using Dialogue;
using Drawing;
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

    [SerializeField]
    private string[] _rejectionDialogues;

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

    private void Start()
    {
        DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
    }

    private void OnDialogueEnded(string conversationID)
    {
        if (conversationID.StartsWith("rejection"))
        {
            ClearDrawing();
            ResumeDrawing();
        }
    }
    public float  GetCompletionPercentage()
    {
        return (float)_currentDrawingIndex / _pictureConfigs.Length;
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

        //Drawing should be reset by the Game Manager
        PauseDrawing();

        if (drawTime < _drawingTime)
        {
            Debug.Log("Drawing is not finished");
            DialogueManager.Instance.StartConversation(
                _rejectionDialogues[UnityEngine.Random.Range(0, _rejectionDialogues.Length)]);
            return;
        }

        PacmanConfig.SetDrawing(_pictureConfigs[_currentDrawingIndex].pictureID, _drawingBase.GetDrawing());
        if (_currentDrawingIndex < _pictureConfigs.Length - 1)
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
        _clearButton.SetInteractable(true);
        _validateButton.SetInteractable(true);
        _drawingUI.SetActive(true);
        _drawingBase.ResumeDrawing();
    }
}