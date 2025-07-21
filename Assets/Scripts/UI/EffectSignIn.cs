using DG.Tweening;
using System;
using UnityEngine;

public class EffectSignIn : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField, Range(0.1f, 1f)]
    private float minScale = 0.7f;
    [SerializeField, Range(0.1f, 1f)]
    private float maxScale = 1f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;
    private void Awake()
    {
        panel.localScale = Vector3.one * minScale;
        panel.gameObject.SetActive(false);
    }
    public void ShowPanel()
    {
        panel.localScale = Vector3.one * minScale;
        panel.gameObject.SetActive(true);
        panel.DOScale(Vector3.one * maxScale, duration).SetEase(showEase).SetUpdate(true);
    }
    public void HidePanel(Action onComplete = null)
    {
        panel.DOScale(Vector3.one * minScale, duration).SetEase(hideEase).SetUpdate(true)
            .OnComplete(() =>
            {
                panel.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }
}
