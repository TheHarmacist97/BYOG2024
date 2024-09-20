
using UnityEngine;

[CreateAssetMenu(menuName = "QTEs/WritingQTEDataHolder")]
public class WritingQTEDataHolder: ScriptableObject
{
    [System.Serializable]
    public class Question
    {
        [TextArea]
        public string question;
        [HideInInspector]
        public string answer;
    }

    public Question[] Questions;
}