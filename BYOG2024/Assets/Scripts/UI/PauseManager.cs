using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private int menuSceneIndex = 0;
    
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Transform titleText;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    
    private bool _isPaused = false;

    private void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(MainMenu);
        
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Pause()
    {
        if(_isPaused)
            Resume();
        else
        {
            _isPaused = true;
            pausePanel.SetActive(true);
            titleText.DOKill();
            titleText.DOShakePosition(0.3f, Vector2.one * 6, 40).SetUpdate(true);
            Time.timeScale = 0f;
        }
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        pausePanel.SetActive(false);
    }
    
    private void MainMenu()
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.LoadHomeScene();
    }
}
