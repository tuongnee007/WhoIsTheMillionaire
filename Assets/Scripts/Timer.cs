using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    [SerializeField] float timeToCompleQuestion = 30f;
    public float timeToShowCorrectAnswer = 10f;
    public bool loadNextQuestion = false;
    public bool isAnsweringQuestion = false;
    float timerValue;
    public float fillFraction;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            UpdateTimer();
        }
    }
    public void CancelTimer()
    {
        timerValue = 0;
    }
    private void UpdateTimer()
    {
        timerValue -= Time.deltaTime;
        if (isAnsweringQuestion)
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToCompleQuestion;
            }
            else
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowCorrectAnswer;
                fillFraction = 1;

            }
        }
        else
        {
            if(timerValue > 0)
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                timerValue = timeToCompleQuestion;
                loadNextQuestion = true;
            }
        }
    }
    public void ResetTimer()
    {
        timerValue = timeToCompleQuestion;
        fillFraction = 1f;
        isAnsweringQuestion = true;
        loadNextQuestion = false;
    }
}
