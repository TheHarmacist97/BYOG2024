using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimerProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient colorGradient;

    [SerializeField] private bool shakeNearEnd = true;
    [Range(0f, 1f)]
    [SerializeField] private float shakeNearEndThreshold = 0.2f;

    [SerializeField] private float shakeIntensity = 2.5f;
    
    private bool _isShaking = false;
    private Tween _shakeTween;

    public void SetProgress(float progress)
    {
        fillImage.fillAmount = progress;
        fillImage.color = colorGradient.Evaluate(progress);
        if (shakeNearEnd && !_isShaking && progress <= shakeNearEndThreshold && progress > 0f)
        {
            _isShaking = true;
            transform.DOScale(Vector3.one * 1.1f, 0.5f);
            _shakeTween = transform.DOShakePosition(100f, new Vector2(shakeIntensity, shakeIntensity),50).SetLoops(-1, LoopType.Yoyo).Play();
        }
        
        if ((_isShaking && progress > shakeNearEndThreshold) || progress <= 0f)
        {
            _isShaking = false;
            transform.DOScale(Vector3.one, 0.2f);
            _shakeTween?.Kill();
        }
    }

    public void ResetTimer()
    {
        _isShaking = false;
        transform.DOScale(Vector3.one, 0.2f);
        _shakeTween?.Kill();
    }

}
