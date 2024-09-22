using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupParent;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Image fillImage;
    [SerializeField] private AnimationCurve fillEaseCurve;

    public CanvasGroup PopupParent => popupParent;
    public void Init(string text, float duration)
    {
        popupText.text = text;
        fillImage.DOFillAmount(1f, duration).SetEase(fillEaseCurve);
    }
    
}
