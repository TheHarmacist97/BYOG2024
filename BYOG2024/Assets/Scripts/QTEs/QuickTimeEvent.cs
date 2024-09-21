using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class QuickTimeEvent : MonoBehaviour
{
    public delegate void OnQTECompleted();
    // event for game manager to listen to
    public event OnQTECompleted onQTECompleted;

    [SerializeField] protected float initialStartDelay = 1.5f;
    
    [SerializeField] protected float totalAllowedTime = 45f;
    // the number of things/actions player has to get right
    [SerializeField] protected int totalActionCount = 10;

    [SerializeField] private string qteDescription;

    [Tooltip("Root UI Panel that belong to this QTE")] 
    [SerializeField] protected GameObject uiPanel;

    protected float _timeLeft = 0f;
    protected bool _isPaused = true;
    protected int _succeededActionCount = 0;
    protected int _failedActionCount = 0;
    protected bool _isComplete = false;

    public bool IsComplete => _isComplete;

    private void Start()
    {
        ToggleAllChildren(false);
    }

    public void StartQTE()
    {
        uiPanel.SetActive(true);
        ToggleAllChildren(true);
        Invoke(nameof(ActuallyStart), initialStartDelay);
    }

    void ActuallyStart()
    {
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
        _isComplete = true;
        OnComplete();
        if (onQTECompleted != null)
            onQTECompleted();
        ToggleAllChildren(false);
    }

    protected virtual void IncrementSuccessAction()
    {
        _succeededActionCount++;
        if(_succeededActionCount + _failedActionCount >= totalActionCount)
            QTEComplete();
    }

    protected virtual void IncrementFailedAction()
    {
        _failedActionCount++;
        if(_succeededActionCount + _failedActionCount >= totalActionCount)
            QTEComplete();
    }

    // Override in child class to carry out on complete activities
    protected abstract void OnComplete();

    public float GetSuccessPercentage()
    {
        return (float)_succeededActionCount / totalActionCount;
    }
    
    public float GetFailurePercentage()
    {
        return (float)_failedActionCount / totalActionCount;
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
        foreach( Transform child in transform )
        {
            child.gameObject.SetActive(enable);
        }
    }
}
