using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class DialogueData 
    {
        public string speaker;
        [TextArea]
        public string dialogueText;
        public Orientation orientation;
    }
}