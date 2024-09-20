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
    
    [SerializeField] private float totalAllowedTime = 45f;
    // the number of things/actions player has to get right
    [SerializeField] private int totalActionCount = 10;

    [Tooltip("Root UI Panel that belong to this QTE")] 
    [SerializeField] private GameObject uiPanel;

    private float _timeLeft = 0f;
    private bool _isPaused = true;
    private int _succeededActionCount = 0;
    private int _failedActionCount = 0;
    private bool _isComplete = false;

    public bool IsComplete => _isComplete;
    
    private void Update()
    {
        if (_isPaused || _isComplete) return;
        
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f)
        {
            QTEComplete();
        }
    }

    private void StartQTE()
    {
        Initialize();
        _isComplete = false;
        _isPaused = false;
        _timeLeft = totalAllowedTime;
        _succeededActionCount = 0;
        _failedActionCount = 0;
        uiPanel.SetActive(true);
    }

    // Override in child class to initialize QTE
    protected abstract void Initialize();

    private void QTEComplete()
    {
        _isComplete = true;
        OnComplete();
        if (onQTECompleted != null)
            onQTECompleted();
    }

    public void IncrementSuccessAction()
    {
        _succeededActionCount++;
        if(_succeededActionCount + _failedActionCount >= totalActionCount)
            QTEComplete();
    }

    public void IncrementFailedAction()
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
    
    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _isPaused = false;
    }
}
