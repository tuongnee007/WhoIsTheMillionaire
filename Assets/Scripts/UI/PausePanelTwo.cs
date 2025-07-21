using DG.Tweening;
using UnityEngine;
public class PausePanelTwo : MonoBehaviour
{
    public static PausePanelTwo Instance;
    public GameObject pausePanel;
    public GameObject backGround;
    public GameObject settingsPanel;
    public EffectSignIn effectPausePanel;
    public EffectPanelSetting effectPanelSetting;
    public GameObject timerPrefab;

    private bool isTransitioning = false;
    private const float transitionDelay = 1f;
    private bool isInSettingPanel = false;
    private void Start()
    {
        if(CountDownTimer.Instance == null)
        {
            Instantiate(timerPrefab);
        }
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        backGround.SetActive(false);

    }
    public void OpenPauseGame()
    {
        if(isTransitioning || isInSettingPanel || DOTween.IsTweening(effectPausePanel.transform))
        {
            return;
        }
        if(!pausePanel.activeSelf)
        {
            isTransitioning = true;
            pausePanel.SetActive(true);
            backGround.SetActive(true);
            effectPausePanel.ShowPanel();
            DOVirtual.DelayedCall(0.01f, () =>
            {
                Time.timeScale = 0f;
            }).SetUpdate(true);
            DOVirtual.DelayedCall(transitionDelay, () =>
            {
                isTransitioning = false;
            }).SetUpdate(true);
        }
        else
        {
            isTransitioning = true;
            effectPausePanel.HidePanel(() =>
            {
                backGround.SetActive(false);
                ResumeGame();
                isTransitioning = false;
            });
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        effectPausePanel.HidePanel(() =>
        {
            backGround.SetActive(false);
            isTransitioning = false;
        });
    }
    public void Restart(int sceneIndex)
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        DOTween.KillAll();
        LevelManager.Instance.LoadLevel(sceneIndex);
    }
    public void BackToMainMenu(int sceneIndex)
    {
        Time.timeScale = 1f;
        PlayerScoreManager.Instance.SendFinalScore();
        PlayerScoreManager.Instance.ResetScore();
        CountDownTimer.Instance.ResetTimer();
        CountDownTimer.Instance.PauseTimer();
        pausePanel.SetActive(false);
        DOTween.KillAll();
        LevelManager.Instance.LoadLevel(sceneIndex);
    }
    public void Settings()
    {
        isTransitioning = true;
        isInSettingPanel = true;
        effectPausePanel.HidePanel(() =>
        {
            pausePanel.SetActive(false);
            settingsPanel.SetActive(true);
            isTransitioning = false;
        });
    }
    public void CloseSettings()
    {
        isTransitioning = true;
        isInSettingPanel = false;
        effectPanelSetting.HidePanel(() =>
        {
            settingsPanel.SetActive(false);
            pausePanel.SetActive(true);
            effectPausePanel.ShowPanel();
            isTransitioning = false; 
        });
    }
}
