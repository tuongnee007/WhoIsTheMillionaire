using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectMainMenu : MonoBehaviour
{
    [Header("Layout & Buttons")]
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private List<RectTransform> buttons;

    [Header("Animation Settings")]
    [SerializeField] private float offsetY = 200f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float stagger = 0.1f;
    [SerializeField] private Ease easeType = Ease.OutBack;

    private float[] targetYs;

    private void Start()
    {
        CacheTargetPositions();
        PrepareStartPositions();

        layoutGroup.enabled = false;

        PlayButtonAnimation();
    }

    private void CacheTargetPositions()
    {
        targetYs = new float[buttons.Count];
        for (int i = 0; i < buttons.Count; i++)
            targetYs[i] = buttons[i].anchoredPosition.y;
    }

    private void PrepareStartPositions()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var rt = buttons[i];
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, targetYs[i] - offsetY);
        }
    }

    private void PlayButtonAnimation()
    {
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < buttons.Count; i++)
        {
            seq.Append(buttons[i]
                .DOAnchorPosY(targetYs[i], duration)
                .SetEase(easeType)
            )
            .AppendInterval(stagger);
        }

        seq.OnComplete(() =>
        {
            layoutGroup.enabled = true;
        });
    }
}
