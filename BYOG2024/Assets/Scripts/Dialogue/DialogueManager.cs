using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public enum Orientation
    {
        Left,
        Right
    }

    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private Dialogue leftDialogue;

        [SerializeField]
        private Dialogue rightDialogue;

        [SerializeField]
        private Conversation[] conversationList;

        private Dictionary<string, Conversation> _conversations;

        private DialogueSequencer _dialogueSequencer;
        private Dialogue _currentDialogue;

        private void Start()
        {
            _conversations = new Dictionary<string, Conversation>();
            _dialogueSequencer = new DialogueSequencer(this, leftDialogue, rightDialogue);
            
            foreach (var conversation in conversationList)
            {
                _conversations.TryAdd(conversation.conversationID, conversation);
            }
        }

        public void StartConversation(string conversationKey)
        {
            if (_conversations.TryGetValue(conversationKey, out var conversation))
            {
                Debug.Log("Starting Conversation: " + conversationKey);
                _dialogueSequencer.Start(conversation);
            }
            else
            {
                Debug.LogError($"Conversation with key {conversationKey} not found");
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _dialogueSequencer.State == DialogueSequencer.ConversationState.Playing)
            {
                _dialogueSequencer.Nudge();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                StartConversation(conversationList[0].conversationID);
            }
        }
    }
}