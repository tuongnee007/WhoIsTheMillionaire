using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Quiz : TuongMonobehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;
    [Header("Anwer")]
    [SerializeField] Button[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;
    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [Header("Timers")]
    [SerializeField] Image timerImage;
    Timer timer;
    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    PlayerScoreManager playerScore => PlayerScoreManager.Instance;

    [Header("PogressBar")]
    [SerializeField] Slider progressBar;
    public bool isComplete;
    [Header("Sound")]
    Sound sound;
    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        sound = FindObjectOfType<Sound>();
        progressBar.value = 0;
        progressBar.maxValue = questions.Count;
    }
    private void Start()
    {
        playerScore.ResetScore();
        timer.loadNextQuestion = true;
    }
    private void Update()
    {
        if (isComplete) return;
        if(timerImage != null)
        {
            timerImage.fillAmount = timer.fillFraction;
        }
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            timer.loadNextQuestion = false;
            GetNextQuestion();
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            hasAnsweredEarly = true;
            DisplayAnswer(-1);
            SetButtonState(false);
            progressBar.value++;
        }
    }
    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        progressBar.value++;
        scoreText.text = $"Điểm: {playerScore.GetScore()}";
    }
    private void DisplayAnswer(int index)
    {
        Image buttonImage;
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Đáp án đúng!";
            sound.SoundAnswerCorrectly();
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            playerScore.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswerText = currentQuestion.GetAnswers(correctAnswerIndex);
            questionText.text = $"Sai rồi, câu trả lời đúng là: {correctAnswerText}";
            sound.SoundAnswerWrong();
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }
    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            playerScore.IncrementQuestionsSeen();
        }
    }
    private void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }
    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswers(i);
        }
    }
    private void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
    private void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
