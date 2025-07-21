using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TuongMonobehaviour : MonoBehaviour
{
    private void Awake()
    {
        LoadComponents();
    }
    private void Reset()
    {
        LoadComponents();
    }
    protected virtual void LoadComponents()
    {

    }
    protected virtual TMP_InputField LoadTMPInputField(TMP_InputField tMP_InputField, string name)
    {
        if (tMP_InputField != null) return tMP_InputField;
        return GameObject.Find(name).GetComponent<TMP_InputField>();
    }
    protected virtual GameObject LoadGameObject(GameObject gameObject, string name)
    {
        if (gameObject != null) return gameObject;
        return GameObject.Find(name);
    }
    protected virtual Image LoadImage(Image image, string name)
    {
        if (image != null) return image;
        return GameObject.Find(name).GetComponent<Image>();
    }
    protected virtual Button LoadButton(Button button, string name)
    {
        if (button != null) return button;
        return GameObject.Find(name).GetComponent<Button>();
    }
    protected virtual TextMeshProUGUI LoadTextMeshProUGUI(TextMeshProUGUI textMeshProUGUI, string name)
    {
        if (textMeshProUGUI != null) return textMeshProUGUI;
        return GameObject.Find(name).GetComponent<TextMeshProUGUI>();
    }
}
