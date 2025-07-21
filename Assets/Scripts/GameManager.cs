using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Quiz quiz;
    EndScreen endScreen;
    public GameObject settingsPanel;
    public GameObject horizontalButton;
    public GameObject horizontalButtonTwo;
    private void Awake()
    {
        quiz = FindObjectOfType<Quiz>();
        endScreen = FindObjectOfType<EndScreen>();
    }
    private void Start()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
    }
    bool hasFinished = false;

    void Update()
    {
        if (quiz.isComplete && !hasFinished)
        {
            hasFinished = true;

            quiz.gameObject.SetActive(false);
            endScreen.gameObject.SetActive(true);

            int finalScore = PlayerScoreManager.Instance.GetScore();
            endScreen.ShowFinalScore(finalScore);

            PlayerScoreManager.Instance.SendFinalScore();
        }
    }

    public void OnReplayLevel()
    {
        LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        PlayerScoreManager.Instance.ResetScore();
        Timer.Instance.ResetTimer();
        horizontalButton.SetActive(false);
        horizontalButtonTwo.SetActive(false);
    }
    public void BackMainMenu()
    {
        LevelManager.Instance.LoadLevel(0);
        horizontalButton.SetActive(false);
        horizontalButtonTwo.SetActive(false);
    }
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Mở giao diện cài đặt");
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1; 
        Debug.Log("Đóng giao diện cài đặt");
    }
}
