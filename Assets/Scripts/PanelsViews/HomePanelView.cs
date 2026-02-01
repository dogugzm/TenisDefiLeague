using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using Cysharp.Threading.Tasks;
using PanelService;
using Sirenix.OdinInspector;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using Views;

namespace PanelsViews
{
    public class HomePanelView : PanelBase, IPanelParameter<HomePanelView.Data>, IUserHeader
    {
        public class Data
        {
            public UserInfoView.Data UserInfoData { get; }
            public List<AnnouncementView.Data> AnnouncementsData { get; }
            public List<LeagueView.Data> JoinedLeaguesData { get; }
            public List<MatchInfoView.Data> UpcomingMatchesData { get; }

            public Data(UserInfoView.Data userInfoData, List<AnnouncementView.Data> announcementsData,
                List<LeagueView.Data> joinedLeaguesData, List<MatchInfoView.Data> upcomingMatchesData)
            {
                UserInfoData = userInfoData;
                AnnouncementsData = announcementsData;
                JoinedLeaguesData = joinedLeaguesData;
                UpcomingMatchesData = upcomingMatchesData;
            }
        }

        [SerializeField] private UserInfoView userInfoView;

        [FoldoutGroup("LEAGUE")] [SerializeField]
        private Transform joinedLeaguesParent;

        [FoldoutGroup("LEAGUE")] [SerializeField]
        private GameObject joinedLeaguesArea;

        [FoldoutGroup("LEAGUE")] [SerializeField]
        private LeagueView leagueInfoViewPrefab;

        [FoldoutGroup("MATCH")] [SerializeField]
        private Transform upcomingMatchesParent;

        [FoldoutGroup("MATCH")] [SerializeField]
        private GameObject upcomingMatchesArea;

        [FoldoutGroup("MATCH")] [SerializeField]
        private MatchInfoView matchInfoViewPrefab;

        [FoldoutGroup("ANNOUNCEMENT")] [SerializeField]
        private Transform announcementsParent;

        [FoldoutGroup("ANNOUNCEMENT")] [SerializeField]
        private GameObject announcementsArea;

        [SerializeField] private Transform HeaderArea;
        [SerializeField] private Image bgImage;

        private IObjectResolver _objectResolver;
        private UIThemeSettings _themeSettings;

        public Data Parameter { get; set; }
        public Transform GetHeaderParent() => HeaderArea;
        public HeaderPanelViewUser.Data HeaderData => new(PanelName, Parameter.UserInfoData);
        public HeaderPanelViewUser HeaderView { get; set; }

        [Inject]
        public void Injection(IObjectResolver objectResolver, UIThemeSettings themeSettings)
        {
            _objectResolver = objectResolver;
            _themeSettings = themeSettings;
        }

        protected override void Awake()
        {
            base.Awake();
            if (bgImage)
            {
                bgImage.color = _themeSettings.PanelBGColor;
            }
        }

        public override async Task ShowAsync()
        {
            userInfoView.InitAsync(Parameter.UserInfoData).Forget();
            await CreateLeagueViews(Parameter.JoinedLeaguesData);
            await CreateMatchViews(Parameter.UpcomingMatchesData);
            await CreateAnnouncementViews(Parameter.AnnouncementsData);
            await base.ShowAsync();
        }

        private async UniTask CreateLeagueViews(List<LeagueView.Data> leagueData)
        {
            if (SetContainerIsNotVisible(leagueData.Count, joinedLeaguesArea))
            {
                ShowPopulerLeagues();
            }
            else
            {
                foreach (var leagueInfo in leagueData)
                {
                    var leagueInfoView = _objectResolver.Instantiate(leagueInfoViewPrefab, joinedLeaguesParent);
                    leagueInfoView.SetData(leagueInfo);
                }
            }
        }

        private UniTask ShowPopulerLeagues()
        {
            //TODO: Implement logic to show popular leagues when the user has no joined leagues.
            return UniTask.CompletedTask;
        }


        private async UniTask CreateMatchViews(List<MatchInfoView.Data> matchData)
        {
            if (SetContainerIsNotVisible(matchData.Count, upcomingMatchesArea)) return;
            foreach (var matchInfo in matchData)
            {
                var matchInfoView = _objectResolver.Instantiate(matchInfoViewPrefab, upcomingMatchesParent);
                matchInfoView.InitAsync(matchInfo).Forget();
            }
        }

        private async UniTask CreateAnnouncementViews(List<AnnouncementView.Data> announcementData)
        {
            if (SetContainerIsNotVisible(announcementData.Count, announcementsArea)) return;
            foreach (var announcementInfo in announcementData)
            {
                // var announcementInfoView = Instantiate(announcementInfoViewPrefab, announcementsParent);
                // announcementInfoView.InitAsync(announcementInfo).Forget();
            }
        }

        private bool SetContainerIsNotVisible(int count, GameObject area)
        {
            if (count == 0)
            {
                area.SetActive(false);
                return true;
            }

            area.SetActive(true);
            return false;
        }
    }
}