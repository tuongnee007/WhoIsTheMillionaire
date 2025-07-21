using PlayFab;
using TMPro;
using UnityEngine;
public class EmailManager : TuongMonobehaviour
{
    public static EmailManager Instance;
    [Header("Sign Up")]
    public TMP_InputField signUpUserName;
    private string getSignUpUserName = "SignUpUserName";
    public TMP_InputField signUpEmail;
    private string getSignUpEmail = "SignUpEmail";
    public TMP_InputField signUpPassword;
    private string getSignUpPassword = "SignUpPassword";
    [Header("Sign In")]
    public TMP_InputField signInEmail;
    private string getSignInEmail = "SignInEmail";
    public TMP_InputField signInPassword;
    private string getSignInPassword = "SignInPassword";
    [Header("Panel")]
    [SerializeField] protected GameObject signUpPanel;
    private string getSignUpPanel = "SignUpPanel";
    public GameObject signInPanel;
    private string getSignInPanel = "SignInPanel";
    [SerializeField] protected GameObject resetPasswordPanel;
    private string getResetPasswordPanel = "ResertPasswordPanel";
    [Header("Reset Password")]
    public TMP_InputField resetPasswordInput;
    private string getEmailInputField = "ResetPasswordEmail";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        LoadComponents();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTMPInputField();
        this.LoadGameObject();
    }
    protected virtual void LoadTMPInputField()
    {
        if (signUpUserName == null) signUpUserName = LoadTMPInputField(signUpUserName, getSignUpUserName);
        if (signUpEmail == null) signUpEmail = LoadTMPInputField(signUpEmail, getSignUpEmail);
        if (signUpPassword == null) signUpPassword = LoadTMPInputField(signUpPassword, getSignUpPassword);
        if (signInEmail == null) signInEmail = LoadTMPInputField(signInEmail, getSignInEmail);
        if (signInPassword == null) signInPassword = LoadTMPInputField(signInPassword, getSignInPassword);
        if (resetPasswordInput == null) resetPasswordInput = LoadTMPInputField(resetPasswordInput, getEmailInputField);
    }
    protected virtual void LoadGameObject()
    {
        if (signUpPanel == null) signUpPanel = LoadGameObject(signUpPanel, getSignUpPanel);
        if (signInPanel == null) signInPanel = LoadGameObject(signInPanel, getSignInPanel);
        if (resetPasswordPanel == null) resetPasswordPanel = LoadGameObject(resetPasswordPanel, getResetPasswordPanel);
    }
    public void LogOut()
    {
        CharacterInformation.Instance.ClearCharacterInfo();
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.SetInt("HasLoggedIn", 0);
        PlayerPrefs.SetInt("AutoLoginDisable", 1);
        PlayerPrefs.Save();
        MainMenu.Instance.PlayMenu.SetActive(true);
        MainMenu.Instance.LogoutButton.SetActive(false);
        MainMenu.Instance.IconLeaderBoard.SetActive(false);
        Debug.Log("Đăng xuất thành công.");
    }
}
