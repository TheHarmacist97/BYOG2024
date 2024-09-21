
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WritingQTE: QuickTimeEvent
{
    [SerializeField]
    private TimerProgressBar timerProgressBar;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private string titlePrefix = "Write The Story ";
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private TMP_InputField answerInputField;
    [SerializeField]
    private Button submitButton;
    
    [SerializeField] private WritingQTEDataHolder writingQTEDataHolder;

    private int _currentQuestionIndex = -1;
    protected override void Initialize()
    {
        _currentQuestionIndex = -1;
        totalActionCount = writingQTEDataHolder.Questions.Length;
        submitButton.onClick.AddListener(SubmitAnswer);
        NextQuestion();
    }

    protected override void OnUpdate()
    {
        timerProgressBar.SetProgress(GetTimeLeftProgress());
    }
    
    private void SubmitAnswer()
    {
        if(answerInputField.text.Length <= 0)
        {
            submitButton.transform.DOShakePosition(0.2f, Vector2.one * 5, 50);
            return;
        }
        writingQTEDataHolder.Questions[_currentQuestionIndex].answer = answerInputField.text;
        IncrementSuccessAction();
        NextQuestion();
    }

    private void NextQuestion()
    {
        if (_currentQuestionIndex >= totalActionCount - 1) return;
        
        _currentQuestionIndex++;
        titleText.text = titlePrefix + "(" + (_currentQuestionIndex + 1) + "/" + totalActionCount + ")";
        questionText.transform.DOShakePosition(0.4f, Vector2.one * 3, 50);
        questionText.text = writingQTEDataHolder.Questions[_currentQuestionIndex].question;
        answerInputField.text = "";
    }

    public WritingQTEDataHolder.Question[] GetQuestionsData()
    {
        return writingQTEDataHolder.Questions;
    }

    protected override void OnComplete()
    {
        timerProgressBar.ResetTimer();
    }
}