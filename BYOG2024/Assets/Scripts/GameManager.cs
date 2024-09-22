using System;
using System.Collections.Generic;
using DG.Tweening;
using Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class QTEBlock
    {
        public Conversation conversation;
        public QuickTimeEvent qte;
        public ApplicationView applicationIcon;
        [HideInInspector]
        public bool executed = false;
    }
    
    [SerializeField]
    private QTEBlock[] departmentLeavingQTEs;

    [SerializeField]
    private Taskbar _taskbar;

    [Header("QTE Panel config")]
    [SerializeField] private RectTransform qteCanvasParent;
    [SerializeField] private float timeToOpenQtePanel;
    [SerializeField] private float qtePanelYOffset;
    [SerializeField] private Ease qtePanelAnimEase = Ease.InSine;
    private QTEBlock _currentQTE;
    private float _qtePanelYPosition;

    private void Start()
    {
        DrawingManager.Instance.DrawingCompleted += OnDrawingCompleted;
        DrawingManager.Instance.AllDrawingsCompleted += OnAllDrawingsCompleted;
        DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
    }

    private void OnAllDrawingsCompleted()
    {
        Debug.Log("All drawings completed");
        SceneManager.LoadScene(3);
    }

    void OnDrawingCompleted(int index)
    {
        if (index >= 1)
        {
            List<QTEBlock> availableQTEs = new List<QTEBlock>(); //Get all available
            foreach (var qte in departmentLeavingQTEs)
            {
                if(!qte.executed)
                    availableQTEs.Add(qte);
            }

            if (availableQTEs.Count > 0)
            {
                _currentQTE = availableQTEs[Random.Range(0, availableQTEs.Count)];
                DialogueManager.Instance.StartConversation(_currentQTE.conversation.conversationID);
            }
        }
        else
        {
            DrawingManager.Instance.StartDrawing();
        }
    }

    void OnDialogueEnded(string conversationID)
    {
        foreach (var qteBlock in departmentLeavingQTEs)
        {
            if (qteBlock.conversation.conversationID.Equals(conversationID))
            {
                qteBlock.executed = true;
                OpenQTEPanel();
                _taskbar.SetApplication(qteBlock.applicationIcon);
                qteBlock.qte.StartQTE();
                qteBlock.qte.onQTECompleted += OnQTEComplete;
            }
                
        }
    }
    
    void OnQTEComplete()
    {
        _taskbar.ResetApplication();
        CloseQTEPanel(_currentQTE.qte.GetUIPanel());
        _currentQTE.qte.onQTECompleted -= OnQTEComplete;
        _currentQTE = null;
        DrawingManager.Instance.StartDrawing();
    }

    void OpenQTEPanel()
    {
        qteCanvasParent.DOMoveY(qtePanelYOffset, 0f);
        qteCanvasParent.localScale = Vector3.zero;
        qteCanvasParent.DOMoveY(0f, timeToOpenQtePanel).SetEase(qtePanelAnimEase);
        qteCanvasParent.DOScale(Vector3.one, timeToOpenQtePanel).SetEase(qtePanelAnimEase);
    }
    
    void CloseQTEPanel(GameObject uiPanel)
    {
        qteCanvasParent.DOMoveY(qtePanelYOffset, timeToOpenQtePanel).SetEase(qtePanelAnimEase);
        qteCanvasParent.DOScale(Vector3.zero, timeToOpenQtePanel).SetEase(qtePanelAnimEase).OnComplete(() =>
        {
            uiPanel.SetActive(false);
        });
    }
}
