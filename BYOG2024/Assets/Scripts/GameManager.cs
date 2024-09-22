using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dialogue;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class QTEBlock
    {
        public Conversation[] conversation;
        public QuickTimeEvent qte;
        public ApplicationView applicationIcon;

        [HideInInspector]
        public bool executed = false;
    }

    [SerializeField]
    private QTEBlock[] departmentLeavingQTEs;

    [Header("Conversations")]
    [SerializeField]
    private string startConversationID;

    [SerializeField]
    private string allDrawingsCompletedConversationID;

    [SerializeField]
    private Taskbar _taskbar;

    [Header("Global Timer")]
    [SerializeField]
    private float maxGameTime = 180f;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private TimerProgressBar timerProgressBar;

    [Header("QTE Panel config")]
    [SerializeField]
    private RectTransform qteCanvasParent;

    [SerializeField]
    private float timeToOpenQtePanel;

    [SerializeField]
    private float qtePanelYOffset;

    [SerializeField]
    private Ease qtePanelAnimEase = Ease.InSine;

    private QTEBlock _currentQTE;
    private float _qtePanelYPosition;
    private float _timeLeft;
    private bool _timeOver = false;
    private bool _pausedTimer = false;

    private IEnumerator Start()
    {
        _timeLeft = maxGameTime;
        DrawingManager.Instance.DrawingCompleted += OnDrawingCompleted;
        DrawingManager.Instance.AllDrawingsCompleted += OnAllDrawingsCompleted;
        DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
        _pausedTimer = true;
        yield return null;
        DialogueManager.Instance.StartConversation(startConversationID);
    }

    private void Update()
    {
        if (!_timeOver && !_pausedTimer)
        {
            _timeLeft -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timeLeft / 60F);
            int seconds = Mathf.FloorToInt(_timeLeft - minutes * 60);

            string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = niceTime;
            timerProgressBar.SetProgress(_timeLeft / maxGameTime);
            if (_timeLeft <= 0f)
            {
                _timeOver = true;
                timerText.text = "00:00";
                timerProgressBar.ResetTimer();
                // GAME OVER STUFF
                GameOver();
                //TODO Different Dialogue
                DialogueManager.Instance.StartConversation(allDrawingsCompletedConversationID);
            }
        }
    }

    private void GameOver()
    {
        //Force Complete all QTEs

        float overallSuccessRate = 0f;
        float programmingSuccessRate = 0f;
        float soundSuccessRate = 0f;
        
        foreach (var qteBlock in departmentLeavingQTEs)
        {
            qteBlock.qte.ForceComplete();
            qteBlock.executed = true;
            switch(qteBlock.qte.ID)
            {
                case "programmingqte":
                    overallSuccessRate += qteBlock.qte.GetSuccessPercentage();
                    programmingSuccessRate = qteBlock.qte.GetSuccessPercentage();
                    break;
                case "soundqte":
                    overallSuccessRate += qteBlock.qte.GetSuccessPercentage();
                    soundSuccessRate = qteBlock.qte.GetSuccessPercentage();
                    break;
            }
        }

        overallSuccessRate += DrawingManager.Instance.GetCompletionPercentage();
        overallSuccessRate /= 3;
        Debug.Log($"Timer Over: {overallSuccessRate}\n Programming: {programmingSuccessRate}\n Sound: {soundSuccessRate}");
        
        
        PacmanConfig.OverallSuccessRate = overallSuccessRate;
        PacmanConfig.ProgrammingSuccess = programmingSuccessRate;
        PacmanConfig.SoundSuccess = soundSuccessRate;
        _taskbar.ResetApplication();
    }

    private void OnAllDrawingsCompleted()
    {
        Debug.Log("All drawings completed");
        _pausedTimer = true;
        GameOver();
        DialogueManager.Instance.StartConversation(allDrawingsCompletedConversationID);
    }

    void OnDrawingCompleted(int index)
    {
        if (index > 1)
        {
            List<QTEBlock> availableQTEs = new List<QTEBlock>(); //Get all available
            foreach (var qte in departmentLeavingQTEs)
            {
                if (!qte.executed)
                    availableQTEs.Add(qte);
            }

            if (availableQTEs.Count > 0)
            {
                _currentQTE = availableQTEs[Random.Range(0, availableQTEs.Count)];
                _pausedTimer = true;
                DialogueManager.Instance.StartConversation(_currentQTE.conversation[Random.Range(0, _currentQTE.conversation.Length)].conversationID);
            }
        }
        else
        {
            DrawingManager.Instance.StartDrawing();
        }
    }

    void OnDialogueEnded(string conversationID)
    {
        if (conversationID.Equals(startConversationID))
        {
            DrawingManager.Instance.StartDrawing();
            _pausedTimer = false;
            return;
        }

        if (conversationID.Equals(allDrawingsCompletedConversationID))
        {
            SceneTransitionManager.Instance.LoadNextScene();
            return;
        }

        foreach (var qteBlock in departmentLeavingQTEs)
        {
            foreach (var conversation in qteBlock.conversation)
            {
                if (!conversation.conversationID.Equals(conversationID)) continue;
                _pausedTimer = false;
                qteBlock.executed = true;
                OpenQTEPanel();
                _taskbar.SetApplication(qteBlock.applicationIcon);
                qteBlock.qte.StartQTE();
                qteBlock.qte.onQTECompleted += OnQTEComplete;
                break;
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