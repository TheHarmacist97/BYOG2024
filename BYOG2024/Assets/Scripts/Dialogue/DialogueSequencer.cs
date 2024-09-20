using System;
using UnityEngine;

namespace Dialogue
{
    public class DialogueSequencer
    {
        public enum ConversationState
        {
            NotStarted,
            Playing,
            Completed
        }

        private readonly DialogueManager _dialogueManager;
        private readonly Dialogue _leftDialogue;
        private readonly Dialogue _rightDialogue;

        private int _currentDialogueIndex;
        private Conversation _conversation;
        private Dialogue _currentDialogue;
        
        private ConversationState _state;
        public ConversationState State => _state;


        public DialogueSequencer(DialogueManager dialogueManager, Dialogue leftDialogue, Dialogue rightDialogue)
        {
            _dialogueManager = dialogueManager;
            _leftDialogue = leftDialogue;
            _rightDialogue = rightDialogue;
            _state = ConversationState.NotStarted;
        }

        public void Start(Conversation conversation)
        {
            _state = ConversationState.Playing;
            _currentDialogueIndex = -1;
            _conversation = conversation;
            ShowNextDialogue();
        }

        public void Nudge()
        {
            Debug.Log("Nudging");
            if (_currentDialogue == null)
            {
                Debug.Log("Current Dialogue is null. Showing Next Dialogue");
                ShowNextDialogue();
            }
            else
            {
                Debug.Log($"Current Dialogue is {_currentDialogue.State}");
                switch (_currentDialogue.State)
                {
                    case Dialogue.DialogueState.Idle:
                        //Dialogue Completed and ready to move to the next one
                        ShowNextDialogue();
                        break;
                    case Dialogue.DialogueState.Playing:
                        _currentDialogue.Nudge();
                        break;
                    case Dialogue.DialogueState.Finished:
                        //Make it fade out
                        _currentDialogue.FadeOutDialogue();
                        ShowNextDialogue();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ShowNextDialogue()
        {
            _currentDialogueIndex++;

            if (_currentDialogueIndex >= _conversation.dialogues.Length)
            {
                //Notify the DialogueManager that the conversation has ended
                _state = ConversationState.Completed;
                return;
            }

            var dialogue = _conversation.dialogues[_currentDialogueIndex];
            if (dialogue.orientation == Orientation.Left)
            {
                Debug.Log("Showing Left Dialogue");
                _currentDialogue = _leftDialogue;
                _leftDialogue.StartDialogue(dialogue);
            }
            else
            {
                Debug.Log("Showing Right Dialogue");
                _currentDialogue = _rightDialogue;
                _rightDialogue.StartDialogue(dialogue);
            }
        }
    }
}