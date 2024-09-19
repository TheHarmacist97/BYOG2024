using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float hoverAnimDuration = 0.4f;
    [SerializeField] private float textSpacingValue;
    private TextMeshProUGUI _text;
    private Vector3 _originalScale;
    private float _initialTextSpacing;

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _originalScale = transform.localScale;
        if(_text)
            _initialTextSpacing = _text.characterSpacing;
    }

    private void OnEnable()
    {
        DOTween.KillAll();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(transform.localScale * hoverScaleMultiplier, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
        if(_text)
            DOTween.To(() => _text.characterSpacing, x => _text.characterSpacing = x, textSpacingValue, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_originalScale, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
        if(_text) 
            DOTween.To(() => _text.characterSpacing, x => _text.characterSpacing = x, _initialTextSpacing, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
    }
}
