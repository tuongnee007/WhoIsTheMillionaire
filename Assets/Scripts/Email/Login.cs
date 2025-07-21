using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
public class Login : EmailManager
{
    private string currentEmail;
    public void SignInGame()
    {
        if (string.IsNullOrEmpty(signInEmail.text))
        {
            NotificationUI.Instance.Show("Vui lòng nhập địa chỉ email.");
            return;
        }
        if (!Regex.IsMatch(signInEmail.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            NotificationUI.Instance.Show("Email không hợp lệ.");
            return;
        }
        if (string.IsNullOrEmpty(signInPassword.text))
        {
            NotificationUI.Instance.Show("Vui lòng nhập mật khẩu.");
            return;
        }
        var request = new LoginWithEmailAddressRequest
        {
            Email = signInEmail.text,
            Password = signInPassword.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSignInSuccess, OnErrorSignIn);
    }
    private void OnSignInSuccess(LoginResult result)
    {
        currentEmail = signInEmail.text;
        PlayerPrefs.SetInt("HasLoggedIn", 1);
        NotificationUI.Instance.Show("Đăng nhập thành công", 2f, () =>
        {
            LinkDeviceAndProceed();
            MainMenu.Instance.ExitPanelLogin();
            StartCoroutine(ClearInput(0.5f));
        });
        ;
    }
    private IEnumerator ClearInput(float delay)
    {
        yield return new WaitForSeconds(delay);
        signInEmail.text = string.Empty;
        signInPassword.text = string.Empty;
    }
    public void LinkDeviceAndProceed()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        var linkRequest = new LinkCustomIDRequest
        {
            CustomId = deviceId,
            ForceLink = true
        };
        PlayFabClientAPI.LinkCustomID(linkRequest,
            success =>
            {
                Debug.Log("Đã liên kết tài khoản với thiết bị");
                OnLoginFinalized();
            },
            error =>
            {
                if (error.Error == PlayFabErrorCode.LinkedDeviceAlreadyClaimed)
                {
                    Debug.Log("Thiết bị đã liên kết tiếp tục ");
                    OnLoginFinalized();
                }
                else
                {
                    Debug.Log("Liên kết thiết bị thất bại: " + error.ErrorMessage);
                }
            });
    }
    private void OnLoginFinalized()
    {
        PlayerPrefs.SetInt("AutoLoginDisable", 0);
        PlayerPrefs.Save();
        if (LeaderBoardCampaign.Instance != null)
            LeaderBoardCampaign.Instance.GetLeaderBoardCampaign();
        CharacterInformation.Instance.ShowCharacters();
    }
    private void OnErrorSignIn(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails != null)
        {
            if (error.ErrorDetails.ContainsKey("Email") || error.ErrorDetails.ContainsKey("Password"))
            {
                NotificationUI.Instance.Show("Email ho?c mật khẩu không đúng.");
                return;
            }
        }
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
            case PlayFabErrorCode.InvalidPassword:
            case PlayFabErrorCode.InvalidEmailAddress:
                NotificationUI.Instance.Show("Email hoặc mật khẩu không đúng.");
                break;
            case PlayFabErrorCode.AccountBanned:
                NotificationUI.Instance.Show("Tài khoản bị khóa.");
                break;
            default:
                NotificationUI.Instance.Show("Email hoặc mật khẩu không đúng.");
                break;
        }
    }
}
