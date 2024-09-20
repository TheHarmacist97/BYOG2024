using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "Custom/Dialogues/Conversation", fileName = "Conversation", order = 1)]
    public class Conversation : ScriptableObject
    {
        public string conversationID;
        public DialogueData[] dialogues;
    }
}