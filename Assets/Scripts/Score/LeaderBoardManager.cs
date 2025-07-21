using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance;
    [SerializeField] protected TextMeshProUGUI playerRankCampignText;
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Transform contentCampign;
    public ScrollRect scrollRectCampign;
    protected const string leaderboardStat = "Quiz";
    protected const string timeStat = "Time";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            RebindText();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected void RebindText()
    {
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        playerRankCampignText = allTexts.FirstOrDefault(t => t.name == "PlayerRankCampaignText");
        var allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();
        canvas = allCanvases.FirstOrDefault(t => t.name == "Canvas");
        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        contentCampign = allTransforms.FirstOrDefault(t => t.name == "ContentCampign");
        var allScrollRects = Resources.FindObjectsOfTypeAll<ScrollRect>();
        scrollRectCampign = allScrollRects.FirstOrDefault(t => t.name == "ScrollRectCampign");
    }
}
