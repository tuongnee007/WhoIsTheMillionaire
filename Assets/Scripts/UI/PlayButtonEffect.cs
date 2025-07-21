using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Audio;
public class PlayButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
    IPointerDownHandler, IPointerUpHandler
{
    public System.Func<bool> IsLockedFunc;
    [Header("Components")]
    public Outline outline;
    public Image targetImage;
    [Header("Color")]
    public Color hoverOutlineColor = new Color32(255, 215, 0, 200); 
    public Color hoverTintColor = new Color32(255, 255, 255, 200);
    private Color originalOutlineColor;
    private Color originalTintColor;
    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float pressedScale = 0.95f; 
    public float scaleTime = 0.1f;
    public AudioMixer mixer;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    [Header("SFXType")]
    public ButtonSFXType buttonSFXType = ButtonSFXType.Confirm;
    public ButtonSFXType buttonSFXTypeTwo = ButtonSFXType.Hover;

    private Tween outlineTween;
    private Tween tintTween;
    private Tween scaleTween;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        if (outline != null)
            originalOutlineColor = outline.effectColor;
        if (targetImage != null)
            originalTintColor = targetImage.color;
    }
    private void OnDisable()
    {
        KillAllTweens();
        DOTween.Kill(rectTransform); 
        DOTween.Kill(targetImage);
        DOTween.Kill(outline);
    }
    private void KillAllTweens()
    {
        outlineTween?.Kill();
        tintTween?.Kill();
        scaleTween?.Kill();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsLockedFunc != null && IsLockedFunc()) return;

        outlineTween?.Kill();
        outlineTween = outline?.DOColor(hoverOutlineColor, 0.2f);
        tintTween?.Kill();
        tintTween = targetImage?.DOColor(hoverTintColor, 0.2f);
        scaleTween?.Kill();
        scaleTween = rectTransform
            .DOScale(originalScale * hoverScale, scaleTime)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
        AudioManagerTwo.Instance.PlayButtonSFX(buttonSFXTypeTwo);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        outlineTween?.Kill();
        outlineTween = outline?.DOColor(originalOutlineColor, 0.2f);
        tintTween?.Kill();
        tintTween = targetImage?.DOColor(originalTintColor, 0.2f);
        scaleTween?.Kill();
        scaleTween = rectTransform
            .DOScale(originalScale , scaleTime)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
    public void OnPointerDown(PointerEventData evt)
    {
        KillAllTweens();

        scaleTween = rectTransform.DOScale(originalScale * pressedScale, scaleTime);
        if (IsLockedFunc != null && IsLockedFunc()) return;

        AudioManagerTwo.Instance.PlayButtonSFX(buttonSFXType);
    }
    public void OnPointerUp(PointerEventData evt)
    {
        bool isHover = RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform, evt.position, evt.enterEventCamera);

        Vector3 targetScale = originalScale * (isHover ? hoverScale : 1f);
        Color targetTint = isHover ? hoverTintColor : originalTintColor;
        Color targetOutln = isHover ? hoverOutlineColor : originalOutlineColor;

        scaleTween?.Kill();
        scaleTween = rectTransform.DOScale(targetScale, scaleTime)
                    .SetEase(Ease.OutBack);
        tintTween?.Kill();
        tintTween = targetImage?.DOColor(targetTint, scaleTime);

        outlineTween?.Kill();
        outlineTween = outline?.DOColor(targetOutln, 0.2f);
    }

}
