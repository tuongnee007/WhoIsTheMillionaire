using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnderlLineOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI label;
    private string originalText;
    private void OnEnable()
    {
        if(label != null) originalText = label.text;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(label != null) label.text = $"<u>{originalText}<u>";
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (label != null) label.text = originalText;
    }

}
