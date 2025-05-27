using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using PanelService;
using TMPro;
using UnityEngine;
using Views;

namespace PanelsViews
{
    public class ProfilePanelView : PanelBase, ITitleHeader, IInitializableAsync<ProfilePanelView.Data>
    {
        public class Data
        {
            public Data(UserInfoView.Data userInfo, int winRate, int totalMatches, int highestRank,
                List<AchievementView.Data> achievements, List<MatchInfoView.Data> matchData)
            {
                UserInfo = userInfo;
                WinRate = winRate;
                TotalMatches = totalMatches;
                HighestRank = highestRank;
                Achievements = achievements;
                MatchData = matchData;
            }

            public UserInfoView.Data UserInfo { get; set; }
            public int WinRate { get; set; }
            public int TotalMatches { get; set; }
            public int HighestRank { get; set; }
            public List<AchievementView.Data> Achievements { get; set; }
            public List<MatchInfoView.Data> MatchData { get; set; }
        }

        [SerializeField] private Transform headerParent;
        [SerializeField] private UserInfoView userInfoView;
        [SerializeField] private TMP_Text winRateText;
        [SerializeField] private TMP_Text totalMatchesText;
        [SerializeField] private TMP_Text highestRankText;

        [SerializeField] private AchievementView achievementViewPrefab;
        [SerializeField] private Transform achievementsParent;

        [SerializeField] private MatchInfoView matchInfoViewPrefab;
        [SerializeField] private Transform matchesParent;

        public Transform GetHeaderParent() => headerParent;

        public HeaderPanelViewTitle.Data HeaderData => new("Profile");

        public HeaderPanelViewTitle HeaderView { get; set; }

        public override Task ShowAsync()
        {
            InitAsync(Parameter).Forget();
            return base.ShowAsync();
        }

        public UniTask InitAsync(Data data)
        {
            Parameter = data;
            userInfoView.InitAsync(data.UserInfo);
            winRateText.text = $"{data.WinRate}%";
            totalMatchesText.text = data.TotalMatches.ToString();
            highestRankText.text = data.HighestRank.ToString();
            foreach (var achievement in data.Achievements)
            {
                var achievementView = Instantiate(achievementViewPrefab, achievementsParent);
                achievementView.InitAsync(achievement);
            }

            foreach (var match in data.MatchData)
            {
                var matchView = Instantiate(matchInfoViewPrefab, matchesParent);
                matchView.InitAsync(match);
            }

            return UniTask.CompletedTask;
        }

        public Data Parameter { get; set; }
    }
}