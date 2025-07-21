using UnityEngine;
using DG.Tweening;
using System;
public class EffectPanelSetting : MonoBehaviour
{
    [SerializeField] private RectTransform panelSetting;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float hideOffetY = 600f;
    private Vector2 originalPos;
    private Vector2 hidePos;
    private void Awake()
    {
        originalPos = panelSetting.anchoredPosition;
    }
    private void OnEnable()
    {
        Vector2 hidePos = originalPos + Vector2.up * hideOffetY;
        panelSetting.DOKill(true);
        panelSetting.anchoredPosition = hidePos;
        panelSetting.DOAnchorPos(originalPos, duration).SetEase(Ease.OutCubic).SetUpdate(true);
    }
    public void ShowPanel()
    {
        panelSetting.DOKill(true);
        Vector2 hidePos = originalPos + Vector2.up * hideOffetY;
        panelSetting.anchoredPosition = hidePos;
        gameObject.SetActive(true);
        MainMenu.Instance.PlayMenu.SetActive(false);
        MainMenu.Instance.SettingPanel.SetActive(true);
        panelSetting.DOAnchorPos(originalPos, duration).SetEase(Ease.OutCubic).SetUpdate(true);
    }
    public void HidePanel(Action onComplete = null)
    {
        Vector2 hidePos = originalPos + Vector2.up * hideOffetY;
        panelSetting.DOKill(true);
        panelSetting.DOAnchorPos(hidePos, duration).SetEase(Ease.InCubic).SetUpdate(true)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }
}
