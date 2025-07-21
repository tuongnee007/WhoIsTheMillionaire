using PlayFab;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;
using UnityEngine;
public class ResetPassword : EmailManager
{
    public void SendRecoveryEmail()
    {
        if (string.IsNullOrEmpty(resetPasswordInput.text))
        {
            NotificationUI.Instance.Show("Vui lòng nhập địa chỉ email");
            return;
        }
        if (!Regex.IsMatch(resetPasswordInput.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            NotificationUI.Instance.Show("Email không hợp lệ.");
            return;
        }
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = resetPasswordInput.text,
            TitleId = PlayFabSettings.staticSettings.TitleId
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnSendRecoveryEmailSuccess, OnSendRecoveryEmailError);
    }
    private void OnSendRecoveryEmailSuccess(SendAccountRecoveryEmailResult result)
    {
        PlayerPrefs.SetString("ResetPasswordEmail", resetPasswordInput.text);
        PlayerPrefs.Save();
        NotificationUI.Instance.Show("Email khôi phục đã được gửi đến " + 
            "Vui lòng kiểm tra hộp thư và đổi mật khẩu, sau đó quay lại đăng nhập.", 7f, () =>
            {
                NotificationUI.Instance.HideImmediately();
                MainMenu.Instance.EffectResetPassword.HidePanel(() =>
                {
                    EmailManager.Instance.resetPasswordInput.text = string.Empty;
                    MainMenu.Instance.LogoutButton.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
                    MainMenu.Instance.ResetPanel.SetActive(false);
                    MainMenu.Instance.LoginPanel.SetActive(true);
                    MainMenu.Instance.EffectLogin.ShowPanel();

                    EmailManager.Instance.signInEmail.text = PlayerPrefs.GetString("ResetPasswordEmail");
                });
            });
    }
    private void OnSendRecoveryEmailError(PlayFabError error)
    {
        NotificationUI.Instance.Show("Lỗi khi gửi email khôi phục", 3f);
    }
}
