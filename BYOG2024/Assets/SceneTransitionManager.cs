
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    [SerializeField] private float _fadeInRate, _fadeOutRate;
    private static readonly int TransitionTime = Shader.PropertyToID("_Progress");
    [SerializeField] private Material _wipeMaterial;
    private Coroutine _wipeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        _wipeMaterial.SetFloat(TransitionTime, 1f);
        StartFadeout();
    }

    private void Start()
    {
        
    }

    public void StartFadeout()
    {
        StartCoroutine(FadeOut());
    }
    [ContextMenu("Fade In")]
    public void StartFadein()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime <= 1f)
        {
            elapsedTime += Time.deltaTime / _fadeOutRate;
            _wipeMaterial.SetFloat(TransitionTime, elapsedTime);
            yield return null;
        }
    }
    private IEnumerator FadeIn()
    {
        float elapsedTime = 1f;
        while (elapsedTime > 0f)
        {
            elapsedTime -= Time.deltaTime / _fadeInRate;
            _wipeMaterial.SetFloat(TransitionTime, elapsedTime);
            yield return null;
        }
    }

    private IEnumerator LoadSceneAfterWipe(int sceneIndex)
    {
        yield return FadeIn();
        SceneManager.LoadScene(sceneIndex);
    }
    
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAfterWipe(sceneIndex));
    }
}
