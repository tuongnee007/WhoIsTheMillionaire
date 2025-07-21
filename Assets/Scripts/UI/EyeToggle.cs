using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EyeToggle : TuongMonobehaviour
{
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button eyeButton;
    [SerializeField] private Sprite eyeOpenIcon;
    [SerializeField] private Sprite eyeClosedIcon;
    [SerializeField] private Image eyeImage;
    private bool isPasswordVisible;
    void Start()
    {
        eyeButton.onClick.AddListener(TogglePasswordVisibility);
        SetPasswordHidden();
    }
    private void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;
        if (isPasswordVisible)
        {
            passwordField.contentType = TMP_InputField.ContentType.Standard;
            eyeImage.sprite = eyeOpenIcon;
        }
        else SetPasswordHidden();
        passwordField.ForceLabelUpdate();
    }
    void SetPasswordHidden()
    {
        passwordField.contentType = TMP_InputField.ContentType.Password;
        eyeImage.sprite = eyeClosedIcon;
    }
}
