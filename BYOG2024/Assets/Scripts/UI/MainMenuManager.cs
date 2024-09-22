using System;
using System.Collections.Generic;
using DG.Tweening;
using Dialogue;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Conversation introConversation;
    [SerializeField] private Button jobAcceptBtn;
    [SerializeField] private Transform scrollingPanel;
    [SerializeField] private float scrollYOffset;
    [SerializeField] private float scrollTime = 0.8f;
    [SerializeField] private Ease scrollEase = Ease.InOutBack;
    
    
    private bool _isPlayed = false;
    private int _escCount = 0;

    private void Start()
    {
        jobAcceptBtn.onClick.AddListener(() =>
        {
            OffsetScrollPanel(StartIntroDialogue);
            jobAcceptBtn.interactable = false;
        });
    }

    private void Update()
    {
        if (!_isPlayed && Input.GetMouseButtonDown(0))
        {
            OffsetScrollPanel();
            _isPlayed = true;
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
            _escCount++;
        
        if(_escCount >= 2)
            Quit();
    }

    private void Play(string conversationId)
    {
        if(conversationId.Equals(introConversation.conversationID))
            SceneTransitionManager.Instance.LoadNextScene();
    }

    private void OffsetScrollPanel(Action OnComplete = null)
    {
        scrollingPanel.DOLocalMoveY(scrollingPanel.localPosition.y + scrollYOffset, scrollTime)
            .SetEase(scrollEase)
            .OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
    }

    private void StartIntroDialogue()
    {
        DialogueManager.Instance.StartConversation(introConversation.conversationID);
        DialogueManager.Instance.OnDialogueEnded += Play;
    }
    
    
    private void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
