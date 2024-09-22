
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PopupManager: MonoBehaviour
{
    [SerializeField]
    private Popup popupPrefab;
    [SerializeField]
    private float popupOpenTime = 0.3f;
    [SerializeField]
    private float popupCloseTime = 0.2f;
    [SerializeField]
    private Ease popupOpenEase = Ease.OutBack;
    public static PopupManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(string text, float duration, Action OnClose = null)
    {
        Popup popup = Instantiate(popupPrefab);
        popup.PopupParent.transform.DOScaleX(0f, 0f);
        popup.PopupParent.transform.DOScaleX(1f, popupOpenTime).SetEase(Ease.OutBack);
        popup.Init(text, duration);
        StartCoroutine(ClosePopup(popup, duration, OnClose));
    }

    IEnumerator ClosePopup(Popup popup, float after, Action OnClose = null)
    {
        yield return new WaitForSeconds(after);
        popup.PopupParent.DOFade(0f, popupCloseTime).OnComplete(() =>
        {
            if (OnClose != null)
                OnClose();
            Destroy(popup.gameObject);
        });
    }
}