using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class QuickTimeEvent : MonoBehaviour
{
    private CancellationTokenSource _cancellationTokenSource;

    public delegate void OnQTECompleted();

    // event for game manager to listen to
    public event OnQTECompleted onQTECompleted;

    [SerializeField]
    protected float initialStartDelay = 1.5f;
    
    [SerializeField]
    protected string completionPopupText = "Completed...";

    [SerializeField]
    protected float totalAllowedTime = 45f;

    // the number of things/actions player has to get right
    [SerializeField]
    protected int totalActionCount = 10;

    [SerializeField]
    private string qteDescription;

    [SerializeField]
    private string qteID;

    [Tooltip("Root UI Panel that belong to this QTE")]
    [SerializeField]
    protected GameObject uiPanel;

    protected float _timeLeft = 0f;
    protected bool _isPaused = true;
    protected int _succeededActionCount = 0;
    protected int _failedActionCount = 0;
    protected bool _isComplete = false;

    public bool IsComplete => _isComplete;
    public string ID => qteID;

    private void Start()
    {
        ToggleAllChildren(false);
    }

    public void StartQTE()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        uiPanel.SetActive(true);
        ToggleAllChildren(true);
        ActuallyStart(_cancellationTokenSource.Token);
    }

    private async void ActuallyStart(CancellationToken cancellationToken)
    {
        await Task.Delay((int)(initialStartDelay * 1000), cancellationToken);
        if (cancellationToken.IsCancellationRequested)
            return;
        Debug.Log("Initializing QTE");
        Initialize();
        _isComplete = false;
        _isPaused = false;
        _timeLeft = totalAllowedTime;
        _succeededActionCount = 0;
        _failedActionCount = 0;
        NotificationManager.Instance.SetNotification(qteDescription);
    }

    private void Update()
    {
        if (_isPaused || _isComplete) return;

        OnUpdate();

        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f)
        {
            QTEComplete();
        }
    }

    protected abstract void OnUpdate();

    // Override in child class to initialize QTE
    protected abstract void Initialize();

    protected void QTEComplete()
    {
        Debug.Log("Completed QTE");
        _isComplete = true;
        PopupManager.Instance.ShowPopup(completionPopupText, 3f, ActuallyComplete);
    }

    void ActuallyComplete()
    {
        OnComplete();
        if (onQTECompleted != null)
            onQTECompleted();
        ToggleAllChildren(false);
    }

    protected virtual void IncrementSuccessAction()
    {
        _succeededActionCount++;
        if (_succeededActionCount + _failedActionCount >= totalActionCount)
            QTEComplete();
    }

    protected virtual void IncrementFailedAction()
    {
        _failedActionCount++;
        if (_succeededActionCount + _failedActionCount >= totalActionCount)
            QTEComplete();
    }

    // Override in child class to carry out on complete activities
    protected abstract void OnComplete();

    public void ForceComplete()
    {
        _cancellationTokenSource?.Cancel();
        QTEComplete();
        uiPanel.SetActive(false);
    }

    public float GetSuccessPercentage()
    {
        return (float)_succeededActionCount / totalActionCount;
    }

    public float GetFailurePercentage()
    {
        return 1 - GetSuccessPercentage();
    }

    public float GetCompletionPercentage()
    {
        return (float)(_succeededActionCount + _failedActionCount) / totalActionCount;
    }

    public float GetTimeLeftProgress()
    {
        return _timeLeft / totalAllowedTime;
    }

    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _isPaused = false;
    }

    public GameObject GetUIPanel()
    {
        return uiPanel;
    }

    private void ToggleAllChildren(bool enable)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(enable);
        }
    }
}