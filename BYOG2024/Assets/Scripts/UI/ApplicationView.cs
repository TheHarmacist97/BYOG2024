using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationView : MonoBehaviour
{
    [SerializeField]
    private Image _iconImage;

    [SerializeField]
    private Image _highlightImage;

    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private Color _selectedColor;

    [SerializeField]
    private Color _unSelectedColor;

    public void SetActive(bool active)
    {
        if (!isActiveAndEnabled && active)
        {
            StartUp();
            return;
        }
            
        if (active)
        {
            _iconImage.rectTransform.DOLocalMoveY(4f, .1f).SetEase(Ease.InSine);
            _highlightImage.rectTransform.DOScaleX(1f, .1f).SetEase(Ease.InSine);
            _highlightImage.DOColor(_selectedColor, 0.1f);
            _backgroundImage.enabled = true;
        }
        else
        {
            _iconImage.rectTransform.DOLocalMoveY(0, .1f).SetEase(Ease.InSine);
            _highlightImage.rectTransform.DOScaleX(0.5f, .1f).SetEase(Ease.InSine);
            _highlightImage.DOColor(_unSelectedColor, 0.1f);
            _backgroundImage.enabled = false;
        }
    }

    public void StartUp()
    {
        Debug.Log("Starting up Application");
        gameObject.SetActive(true);
        _backgroundImage.enabled = true;
        _highlightImage.transform.DOScaleX(1f, .1f).SetEase(Ease.InSine);
        _iconImage.rectTransform.DOLocalMoveY(4f, .1f).SetEase(Ease.OutBounce);
        _highlightImage.DOColor(_selectedColor, 0.1f);
    }

    // private bool test;

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         SetActive(test = !test);
    //     }
    // }
}