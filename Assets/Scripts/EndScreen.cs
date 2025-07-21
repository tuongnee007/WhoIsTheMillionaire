using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    public static EndScreen Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void ShowFinalScore(int finalScore)
    {
        finalScoreText.text = "Chúc mừng bạn, bạn đã đạt số điểm là " + finalScore;
    }
}
