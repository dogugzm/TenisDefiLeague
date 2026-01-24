using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using Cysharp.Threading.Tasks;
using PanelService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Views;

namespace PanelsViews
{
    public class ProfilePanelView : PanelBase, ITitleHeader, IPanelParameter<ProfilePanelView.Data>
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

        [Inject] private UIThemeSettings _themeSettings;

        [SerializeField] private Transform headerParent;
        [SerializeField] private UserInfoView userInfoView;
        [SerializeField] private TMP_Text winRateText;
        [SerializeField] private TMP_Text totalMatchesText;
        [SerializeField] private TMP_Text highestRankText;

        [SerializeField] private AchievementView achievementViewPrefab;
        [SerializeField] private Transform achievementsParent;

        [SerializeField] private MatchInfoView matchInfoViewPrefab;
        [SerializeField] private Transform matchesParent;
        [SerializeField] private Image bgImage;

        public Transform GetHeaderParent() => headerParent;

        public HeaderPanelViewTitle.Data HeaderData => new("Profile");

        public HeaderPanelViewTitle HeaderView { get; set; }

        protected override void Awake()
        {
            base.Awake();
            if (bgImage)
            {
                bgImage.color = _themeSettings.PanelBGColor;
            }
        }

        public override Task ShowAsync()
        {
            InitAsync().Forget();
            return base.ShowAsync();
        }

        private UniTask InitAsync()
        {
            userInfoView.InitAsync(Parameter.UserInfo);
            winRateText.text = $"{Parameter.WinRate}%";
            totalMatchesText.text = Parameter.TotalMatches.ToString();
            highestRankText.text = Parameter.HighestRank.ToString();

            if (Parameter.Achievements is not null)
            {
                foreach (var achievement in Parameter.Achievements)
                {
                    var achievementView = Instantiate(achievementViewPrefab, achievementsParent);
                    achievementView.InitAsync(achievement);
                }
            }

            foreach (var match in Parameter.MatchData)
            {
                var matchView = Instantiate(matchInfoViewPrefab, matchesParent);
                matchView.InitAsync(match);
            }

            return UniTask.CompletedTask;
        }

        public Data Parameter { get; set; }
    }
}