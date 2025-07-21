using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(ScrollRect))]
public class SmoothMouseScroll : MonoBehaviour
{
    public float scrollSpeed = 3f;
    public float smoothTime = 0.1f;

    private ScrollRect scrollRect;
    private float targetPosition;
    private float velocity;
    private RectTransform viewport;

    private bool initialized = false;
    private bool userScrolled = false;

    private float lastFramePosition;
    private float handDragStartTime;
    private bool possibleHandDrag = false;

    private const float handDragThreshold = 0.005f;
    private const float handDragConfirmTime = 0.1f;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        initialized = false;
        userScrolled = false;
        StartCoroutine(DelayScrollInit());
    }
    private IEnumerator DelayScrollInit()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        targetPosition = scrollRect.verticalNormalizedPosition;
        lastFramePosition = targetPosition;
        initialized = true;
    }
    void Update()
    {
        if (!initialized) return;

        float currentPos = scrollRect.verticalNormalizedPosition;
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (IsMouseOver() && Mathf.Abs(scrollDelta) > 0.01f)
        {
            targetPosition += scrollDelta * scrollSpeed;
            targetPosition = Mathf.Clamp01(targetPosition);
            userScrolled = true;
            possibleHandDrag = false;
        }
        else if (userScrolled)
        {
            float delta = Mathf.Abs(currentPos - lastFramePosition);

            if (delta > handDragThreshold)
            {
                if (!possibleHandDrag)
                {
                    possibleHandDrag = true;
                    handDragStartTime = Time.time;
                }
                else if (Time.time - handDragStartTime >= handDragConfirmTime)
                {
                    userScrolled = false;
                    targetPosition = currentPos;
                    possibleHandDrag = false;
                }
            }
            else
            {
                possibleHandDrag = false;
            }
        }

        lastFramePosition = currentPos;

        if (userScrolled)
        {
            scrollRect.verticalNormalizedPosition = Mathf.SmoothDamp(
                currentPos,
                targetPosition,
                ref velocity,
                smoothTime
            );
        }
    }
    bool IsMouseOver()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(viewport, Input.mousePosition, null);
    }
}
