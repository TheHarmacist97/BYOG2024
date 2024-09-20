using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnButtonDownTween : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _hoverScaleMultiplier = 1.2f;

    [SerializeField]
    private float _pressedScaleMultiplier = .95f;
    [SerializeField]
    private float _animationDuration;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(_pressedScaleMultiplier, _animationDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, _animationDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_hoverScaleMultiplier, _animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, _animationDuration);
    }
}
