using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class LeaderBoardCampaign : LeaderBoardManager
{
    [SerializeField] private GameObject entryPrefabCampign;
    public new static LeaderBoardCampaign Instance;
    private Dictionary<string, DateTime> createdMapCache = new(); // Dictionary là bảng ánh xạ, id - ngày tạo tài khoản
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void GetMyRank()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("Chưa đăng nhập vào PlayFab, không thể lấy bảng xếp hạng.");
            return;
        }
        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardStat,
            MaxResultsCount = 1
        }, result =>
        {
            if (result.Leaderboard?.Count > 0) StartCoroutine(LoadRank(result.Leaderboard[0]));
            else playerRankCampignText.text = "Không tìm thấy thứ hạng.";
        }, error =>
        {
            playerRankCampignText.text = "Lỗi khi lấy hạng.";
            Debug.LogError("Lỗi lấy bảng xếp hạng xung quanh người chơi: " + error.GenerateErrorReport());
        });
    }
    private IEnumerator LoadRank(PlayerLeaderboardEntry myEntry)
    {
        var topEntries = new List<PlayerLeaderboardEntry>();
        var timeMap = new Dictionary<string, int>();
        var createdMap = new Dictionary<string, DateTime>();
        bool doneScores = false;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = leaderboardStat,
            StartPosition = 0,
            MaxResultsCount = 100
        },
        res => { topEntries = res.Leaderboard.ToList(); doneScores = true; },
        err => { Debug.LogError(err.GenerateErrorReport()); doneScores = true; });
        yield return new WaitUntil(() => doneScores);
        if (!topEntries.Any(e => e.PlayFabId == myEntry.PlayFabId))
            topEntries.Add(myEntry);
        bool doneTimes = false;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = timeStat,
            StartPosition = 0,
            MaxResultsCount = 100
        }, res => { foreach (var e in res.Leaderboard) timeMap[e.PlayFabId] = e.StatValue; doneTimes = true; },
        err => { Debug.LogWarning(err.GenerateErrorReport()); doneTimes = true; });
        yield return new WaitUntil(() => doneTimes);
        if (!timeMap.ContainsKey(myEntry.PlayFabId))
        {
            bool doneSelfTime = false;
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
            {
                StatisticNames = new List<string> { timeStat }
            }, res =>
            {
                var stat = res.Statistics.FirstOrDefault(s => s.StatisticName == timeStat);
                timeMap[myEntry.PlayFabId] = stat?.Value ?? int.MaxValue; doneSelfTime = true;
            }, err => { timeMap[myEntry.PlayFabId] = int.MaxValue; doneSelfTime = true; });
            yield return new WaitUntil(() => doneSelfTime);
        }
        int pending = topEntries.Count;
        bool doneAccounts = false;
        foreach (var entry in topEntries)
        {
            string id = entry.PlayFabId;
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = id },
            res =>
            {
                createdMap[id] = res.AccountInfo.Created; createdMapCache[id] = res.AccountInfo.Created;
                if (--pending == 0) doneAccounts = true;
            }, err =>
            {
                createdMap[id] = DateTime.MaxValue;
                if (--pending == 0) doneAccounts = true;
            });
        }
        yield return new WaitUntil(() => doneAccounts);
        topEntries.Sort((a, b) => CompareRank(a, b, timeMap, createdMap));
        int myIndex = topEntries.FindIndex(e => e.PlayFabId == myEntry.PlayFabId);
        string name = string.IsNullOrEmpty(myEntry.DisplayName) ? "Bạn" : myEntry.DisplayName;
        playerRankCampignText.text = $"{name} đang đứng hạng: {myIndex + 1}";
    }
    public void GetLeaderBoardCampaign()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("Chưa đăng nhập vào PlayFab, không thể lấy bảng xếp hạng.");
            return;
        }
        RebindText();
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = leaderboardStat,
            StartPosition = 0,
            MaxResultsCount = 100
        },
        result => StartCoroutine(GetLeaderboardTime(result.Leaderboard)),
        error => Debug.LogError("Lỗi khi lấy bảng xếp hạng: " + error.GenerateErrorReport()));
    }
    private IEnumerator GetLeaderboardTime(List<PlayerLeaderboardEntry> entries)
    {
        Dictionary<string, int> timeMap = new();
        Dictionary<string, DateTime> createdMap = new();
        bool doneTime = false;
        bool doneCreated = false;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = timeStat,
            StartPosition = 0,
            MaxResultsCount = 100
        },
        result =>
        {
            foreach (var e in result.Leaderboard)
                timeMap[e.PlayFabId] = e.StatValue;
            doneTime = true;
        },
        error => { doneTime = true; });
        yield return new WaitUntil(() => doneTime);
        int pending = entries.Count;
        foreach (var entry in entries)
        {
            string id = entry.PlayFabId;
            if (createdMapCache.TryGetValue(id, out DateTime cached))
            {
                createdMap[id] = cached;
                if (--pending == 0) doneCreated = true;
                continue;
            }
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = id },
            res =>
            {
                createdMap[id] = res.AccountInfo.Created;
                createdMapCache[id] = res.AccountInfo.Created;
                if (--pending == 0) doneCreated = true;
            },
            err =>
            {
                createdMap[id] = DateTime.MaxValue;
                if (--pending == 0) doneCreated = true;
            });
        }
        yield return new WaitUntil(() => doneCreated);
        entries.Sort((a, b) => CompareRank(a, b, timeMap, createdMap));
        DisplayLeaderboard(entries, timeMap);
    }
    private static int CompareRank(PlayerLeaderboardEntry a, PlayerLeaderboardEntry b, Dictionary<string, int> timeMap, Dictionary<string, DateTime> createdMap)
    {
        int cmp = b.StatValue.CompareTo(a.StatValue);
        if (cmp != 0) return cmp;
        timeMap.TryGetValue(a.PlayFabId, out int ta);
        timeMap.TryGetValue(b.PlayFabId, out int tb);
        cmp = ta.CompareTo(tb);
        if (cmp != 0) return cmp;
        createdMap.TryGetValue(a.PlayFabId, out DateTime ca);
        createdMap.TryGetValue(b.PlayFabId, out DateTime cb);
        return ca.CompareTo(cb);
    }
    private void DisplayLeaderboard(List<PlayerLeaderboardEntry> entries, Dictionary<string, int> timeMap)
    {
        if(contentCampign == null)
        {
            Debug.Log("ContentCampign is not set, cannot display leaderboard.");
            return;
        }
        foreach (Transform child in contentCampign)
            Destroy(child.gameObject);

        string currentId = PlayFabSettings.staticPlayer.PlayFabId;

        for (int i = 0; i < entries.Count; i++)
        {
            var e = entries[i];
            GameObject go = Instantiate(entryPrefabCampign, contentCampign);
            var texts = go.GetComponentsInChildren<TextMeshProUGUI>();

            string time = timeMap.TryGetValue(e.PlayFabId, out int t) ? FormatTime(t) : "??:??";

            texts[0].text = (i + 1).ToString();
            texts[1].text = e.DisplayName ?? "No name";
            texts[2].text = e.StatValue.ToString();
            texts[3].text = time;
            if (e.PlayFabId == currentId)
                go.GetComponent<Image>().color = new Color32(0xEF, 0xC9, 0x02, 0xFF);
            else
                go.GetComponent<Image>().color = Color.clear;
        }
    }
    private string FormatTime(int totalSeconds)
    {
        int h = totalSeconds / 3600, m = (totalSeconds % 3600) / 60, s = totalSeconds % 60;
        //return $"{h:D2}:{m:D2}:{s:D2}";
        return $"{m:D2}:{s:D2}";
    }
    public void SendScoreCampaign(int value)
    {
        int sessionTime = CountDownTimer.Instance?.GetSessionDurationInSeconds() ?? 0;
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string> { leaderboardStat, timeStat }
        },
        result =>
        {
            int? currentScore = null;
            int? currentTime = null;
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == leaderboardStat)
                    currentScore = stat.Value;
                else if (stat.StatisticName == timeStat)
                    currentTime = stat.Value;
            }
            bool shouldUpdateScore = currentScore == null || value > currentScore;
            bool shouldUpdateTime = false;
            if (value == currentScore && (currentTime == null || sessionTime < currentTime))
                shouldUpdateTime = true;
            var stats = new List<StatisticUpdate>();
            if (shouldUpdateScore)
            {
                stats.Add(new StatisticUpdate { StatisticName = leaderboardStat, Value = value });
                stats.Add(new StatisticUpdate { StatisticName = timeStat, Value = sessionTime });
            }
            else if (shouldUpdateTime)
            {
                stats.Add(new StatisticUpdate { StatisticName = timeStat, Value = sessionTime });
            }
            else
            {
                Debug.Log("Không cập nhật vì không có thông tin nào tốt hơn.");
                return;
            }
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest { Statistics = stats },
                result =>
                {
                    Debug.Log("Cập nhật điểm/thời gian thành công");
                    GetLeaderBoardCampaign();
                },
                error => Debug.LogError("Lỗi gửi điểm: " + error.GenerateErrorReport()));
        },
        error => Debug.LogError("Lỗi khi lấy thống kê: " + error.GenerateErrorReport()));
    }
    public void EnsureDefaultScore()
    {
        var stats = new List<StatisticUpdate>
        {
            new() { StatisticName = leaderboardStat, Value = 0 },
            new() { StatisticName = timeStat, Value = 0 }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest { Statistics = stats },
            result => Debug.Log("Đã đăng ký điểm 0 ban đầu"),
            error => Debug.LogError("Lỗi gửi điểm mặc định: " + error.GenerateErrorReport()));
    }
}