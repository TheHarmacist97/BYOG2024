using System;
using UnityEngine;

public class Taskbar : MonoBehaviour
{
    [SerializeField]
    private ApplicationView[] _applicationViews;

    [SerializeField]
    private ApplicationView _drawingApplicationView;

    private ApplicationView _currentApplication;

    public void SetApplication(ApplicationView applicationView)
    {
        if(_currentApplication != null)
            _currentApplication.SetActive(false);
        
        _drawingApplicationView.SetActive(false);
        _currentApplication = applicationView;
        _currentApplication.SetActive(true);
    }

    public void ResetApplication()
    {
        _currentApplication.SetActive(false);
        _currentApplication = null;
        _drawingApplicationView.SetActive(true);
    }

    private void Start()
    {
        _drawingApplicationView.SetActive(true);
    }
}
