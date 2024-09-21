using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class QTEBlock
    {
        public Conversation conversation;
        public QuickTimeEvent qte;
        [HideInInspector]
        public bool executed = false;
    }
    
    [SerializeField]
    private QTEBlock[] departmentLeavingQTEs;

    private QTEBlock _currentQTE;

    private void Start()
    {
        DrawingManager.Instance.DrawingCompleted += OnDrawingCompleted;
        DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
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
                DrawingManager.Instance.PauseDrawing();
                DialogueManager.Instance.StartConversation(_currentQTE.conversation.conversationID);
            }
        }
    }

    void OnDialogueEnded(string conversationID)
    {
        foreach (var qteBlock in departmentLeavingQTEs)
        {
            if (qteBlock.conversation.conversationID.Equals(conversationID))
            {
                qteBlock.executed = true;
                qteBlock.qte.StartQTE();
                qteBlock.qte.onQTECompleted += OnQUEComplete;
            }
                
        }
    }
    
    void OnQUEComplete()
    {
        _currentQTE.qte.onQTECompleted -= OnQUEComplete;
        _currentQTE = null;
        DrawingManager.Instance.ResumeDrawing();
    }
}
