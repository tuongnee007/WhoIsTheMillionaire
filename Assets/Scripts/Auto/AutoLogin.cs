using System.Collections;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class AutoLogin : MonoBehaviour
{
    public static AutoLogin Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("AutoLoginDisable = " + PlayerPrefs.GetInt("AutoLoginDisable"));

        bool autoLoginDisable = PlayerPrefs.GetInt("AutoLoginDisable", 0) == 1;
        if (!autoLoginDisable)
        {
            LoginWithCustomID();
        }
    }

    public void LoginWithCustomID()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailured);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Auto Login thành công");
        PlayerPrefs.SetInt("HasLoggedIn", 1);
        PlayerPrefs.Save();

        CharacterInformation.Instance?.ShowCharacters();

        if (MainMenu.Instance != null)
        {
            MainMenu.Instance.LogoutButton.SetActive(true);
            MainMenu.Instance.PlayMenu.SetActive(true);
        }
        StartCoroutine(DeleyAndLoadLeaderBoard());
    }

    private IEnumerator DeleyAndLoadLeaderBoard()
    {
        yield return new WaitUntil(() =>
            PlayFabClientAPI.IsClientLoggedIn() &&
            LeaderBoardCampaign.Instance != null);

        Debug.Log("Auto Login hoàn tất, đang tải LeaderBoard");
        LeaderBoardCampaign.Instance.GetLeaderBoardCampaign();
        LeaderBoardCampaign.Instance.GetMyRank();
    }

    private void OnLoginFailured(PlayFabError error)
    {
        Debug.Log("Lỗi Auto đăng nhập: " + error.GenerateErrorReport());
        PlayerPrefs.SetInt("HasLoggedIn", 0);
        PlayerPrefs.Save();
    }
}