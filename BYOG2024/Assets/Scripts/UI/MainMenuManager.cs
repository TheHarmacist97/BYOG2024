using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button quitButton;
    
    [Header("Options")]
    [SerializeField] private CanvasGroup optionsPanel;
    [SerializeField] private Button optionsBackButton;
    
    [Header("Tutorial")]
    [SerializeField] private CanvasGroup tutorialPanel;
    [SerializeField] private Button tutorialBackButton;
    [SerializeField] private Transform tutorialsHolderObj;
    [SerializeField] private Button tutorialPrevButton;
    [SerializeField] private Button tutorialNextButton;
    
    private int _currentTutorialIndex = 0;
    private List<GameObject> _tutorials = new List<GameObject>();

    private void Start()
    {
        playButton.onClick.AddListener(Play);
        optionsButton.onClick.AddListener(ShowOptionsMenu);
        tutorialButton.onClick.AddListener(ShowTutorialMenu);
        quitButton.onClick.AddListener(Quit);
        optionsBackButton.onClick.AddListener(CloseOptionsMenu);
        tutorialBackButton.onClick.AddListener(CloseTutorialMenu);
        tutorialNextButton.onClick.AddListener(NextTutorial);
        tutorialPrevButton.onClick.AddListener(PrevTutorial);
        
        optionsPanel.gameObject.SetActive(false);
        tutorialPanel.gameObject.SetActive(false);

        foreach (Transform tutorial in tutorialsHolderObj)
        {
            _tutorials.Add(tutorial.gameObject);
            tutorial.gameObject.SetActive(false);
        }

        _tutorials[_currentTutorialIndex].SetActive(true);
    }
    
    private void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void ShowOptionsMenu()
    {
        optionsPanel.gameObject.SetActive(true);
        optionsPanel.alpha = 0f;
        optionsPanel.transform.localScale *= 0.8f;
        optionsPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        optionsPanel.DOFade(1f, 0.3f);
    }

    private void CloseOptionsMenu()
    {
        optionsPanel.transform.DOScale(0.8f, 0.3f);
        optionsPanel.DOFade(0f, 0.3f).OnComplete(() =>
        {
            optionsPanel.gameObject.SetActive(false);
        });
    }
    
    private void ShowTutorialMenu()
    {
        tutorialPanel.gameObject.SetActive(true);
        tutorialPanel.alpha = 0f;
        tutorialPanel.transform.localScale *= 0.8f;
        tutorialPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        tutorialPanel.DOFade(1f, 0.3f);
    }

    private void CloseTutorialMenu()
    {
        tutorialPanel.transform.DOScale(0.8f, 0.3f);
        tutorialPanel.DOFade(0f, 0.3f).OnComplete(() =>
        {
            tutorialPanel.gameObject.SetActive(false);
        });
    }
    
    private void NextTutorial()
    {
        if(_tutorials.Count <= _currentTutorialIndex + 1)
            return;
        
        _tutorials[_currentTutorialIndex].SetActive(false);
        _currentTutorialIndex++;
        _tutorials[_currentTutorialIndex].SetActive(true);

        if (_tutorials.Count <= _currentTutorialIndex + 1)
        {
            tutorialNextButton.interactable = false;
        }

        if (_currentTutorialIndex > 0)
        {
            tutorialPrevButton.interactable = true;
        }
    }
    
    private void PrevTutorial()
    {
        if(_currentTutorialIndex <= 0)
            return;
        
        _tutorials[_currentTutorialIndex].SetActive(false);
        _currentTutorialIndex--;
        _tutorials[_currentTutorialIndex].SetActive(true);
        
        if (_currentTutorialIndex <= 0)
        {
            tutorialPrevButton.interactable = false;
        }

        if (_tutorials.Count > _currentTutorialIndex + 1)
        {
            tutorialNextButton.interactable = true;
        }
    }
    
    private void Quit()
    {
        Application.Quit();
    }
}
