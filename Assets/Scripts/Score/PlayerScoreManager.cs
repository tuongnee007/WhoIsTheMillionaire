using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance;

    private int totalScore = 0;
    private int correctAnswers = 0;
    private int questionsSeen = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
        totalScore += 100;
    }

    public void IncrementQuestionsSeen()
    {
        questionsSeen++;
    }

    public int GetScore() => totalScore;
    public int GetCorrectAnswers() => correctAnswers;
    public int GetQuestionsSeen() => questionsSeen;

    public void ResetScore()
    {
        totalScore = 0;
        correctAnswers = 0;
        questionsSeen = 0;
    }

    public void SendFinalScore()
    {
        LeaderBoardCampaign.Instance.SendScoreCampaign(totalScore);
    }
    private void OnApplicationQuit()
    {
        if (totalScore > 0)
        {
            SendFinalScore();
            ResetScore();
        }
    }
}
