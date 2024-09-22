using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float hoverAnimDuration = 0.4f;
    [SerializeField] private float textSpacingValue;

    private TextMeshProUGUI _text;
    private Vector3 _originalScale;
    private float _initialTextSpacing;
    private Button _button;

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _originalScale = transform.localScale;
        if(_text)
            _initialTextSpacing = _text.characterSpacing;
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        DOTween.KillAll();
    }

    public void SetInteractable(bool interactable)
    {
        if(_button == null)
            _button = GetComponent<Button>();
        _button.interactable = interactable;
        if(!_button.interactable)
            transform.DOScale(_originalScale, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!_button.interactable) return;
        transform.DOScale(transform.localScale * hoverScaleMultiplier, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
        if(_text)
            DOTween.To(() => _text.characterSpacing, x => _text.characterSpacing = x, textSpacingValue, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!_button.interactable) return;
        transform.DOScale(_originalScale, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
        if(_text) 
            DOTween.To(() => _text.characterSpacing, x => _text.characterSpacing = x, _initialTextSpacing, hoverAnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!_button.interactable) return;
        AudioManager.instance.PlaySound2D("CLICK");
    }
}
