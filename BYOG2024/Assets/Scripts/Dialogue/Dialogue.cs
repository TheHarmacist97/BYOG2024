using DG.Tweening;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class Dialogue : MonoBehaviour
    {
        public enum DialogueState
        {
            Idle,
            Playing,
            Finished,
        }

        [SerializeField]
        private TypewriterByWord typewriterByWord;

        [SerializeField]
        private TextMeshProUGUI speaker;

        [SerializeField]
        private CanvasGroup canvasGroup;

        public DialogueState State { get; private set; }

        private void Start()
        {
            typewriterByWord.onTextShowed.AddListener(OnTextShownCompleted);
            typewriterByWord.SetTypewriterSpeed(2f);
        }

        public void StartDialogue(DialogueData dialogue)
        {
            State = DialogueState.Playing;
            speaker.SetText(dialogue.speaker);
            typewriterByWord.ShowText(dialogue.dialogueText);
            canvasGroup.transform.DOShakePosition(0.4f, Vector2.one * 3, 50);
            canvasGroup.DOKill();
            canvasGroup.DOFade(1, 0.1f);
        }

        public void Nudge()
        {
            if (State == DialogueState.Playing)
                typewriterByWord.SkipTypewriter();
        }

        public void FadeOutDialogue()
        {
            State = DialogueState.Idle;
            canvasGroup.DOKill();
            canvasGroup.DOFade(0, .1f);
        }

        private void OnTextShownCompleted()
        {
            State = DialogueState.Finished;
        }
    }
}