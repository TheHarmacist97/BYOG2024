using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class QTEBlock
    {
        public Conversation conversation;
        public QuickTimeEvent qte;
    }
    [SerializeField]
    private QTEBlock[] departmentLeavingQTEs;

    private void Start()
    {
        DrawingManager.Instance.DrawingCompleted += OnDrawingCompleted;
    }
    
    void OnDrawingCompleted(int index)
    {
        
    }
}
