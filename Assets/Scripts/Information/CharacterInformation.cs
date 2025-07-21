using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInformation : MonoBehaviour
{
    public static CharacterInformation Instance;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterId;
    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        LoadOrFetCharacterInfo();
    }
    private void LoadOrFetCharacterInfo()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            ShowCharacters();
            return;
        }

        string savedName = PlayerPrefs.GetString("Name", "");
        string savedId = PlayerPrefs.GetString("Id", "");

        if (!string.IsNullOrEmpty(savedName) || !string.IsNullOrEmpty(savedId))
        {
            characterName.text = savedName;
            characterId.text = "ID: " + savedId;
        }
        else
        {
            characterName.text = "Vui lòng đăng nhập để xem thông tin";
            characterId.text = "";
        }
    }
    public void ShowCharacters()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            string displayName = result.AccountInfo.TitleInfo.DisplayName;
            characterName.text = "Name: " + displayName;
            string userID = result.AccountInfo.PlayFabId;
            characterId.text = "ID: " + userID;
            PlayerPrefs.SetString("Name", displayName);
            PlayerPrefs.SetString("Id", userID);
            PlayerPrefs.Save();

        }, error =>
        {
            characterName.text = "Lỗi khi lấy tên người dùng: " + error.GenerateErrorReport();
        });
    }
    public void ClearCharacterInfo()
    {
        characterName.text = "Vui lòng đăng nhập để xem thông tin";
        characterId.text = "";
        PlayerPrefs.DeleteKey("Name");
        PlayerPrefs.DeleteKey("Id");
        PlayerPrefs.Save();
    }
}
