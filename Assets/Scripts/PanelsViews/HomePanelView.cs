using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.PanelService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views;

namespace PanelsViews
{
    public class HomePanelView : PanelBase, IPanelParameter<HomePanelView.Data>
    {
        public class Data
        {
            public UserInfoView.Data UserInfoData { get; }
            public List<AnnouncementView.Data> AnnouncementsData { get; }
            public List<LeagueInfoView.Data> JoinedLeaguesData { get; }
            public List<MatchInfoView.Data> UpcomingMatchesData { get; }

            public Data(UserInfoView.Data userInfoData, List<AnnouncementView.Data> announcementsData,
                List<LeagueInfoView.Data> joinedLeaguesData, List<MatchInfoView.Data> upcomingMatchesData)
            {
                UserInfoData = userInfoData;
                AnnouncementsData = announcementsData;
                JoinedLeaguesData = joinedLeaguesData;
                UpcomingMatchesData = upcomingMatchesData;
            }
        }

        [SerializeField] private UserInfoView userInfoView;
        [SerializeField] private Transform joinedLeaguesParent;
        [SerializeField] private Transform upcomingMatchesParent;
        [SerializeField] private Transform announcementsParent;
        [SerializeField] private LeagueInfoView leagueInfoViewPrefab;
        [SerializeField] private MatchInfoView matchInfoViewPrefab;


        public Data Parameter { get; set; }

        public override async Task ShowAsync()
        {
            userInfoView.InitAsync(Parameter.UserInfoData).Forget();
            await CreateLeagueViews(Parameter.JoinedLeaguesData);
            await CreateMatchViews(Parameter.UpcomingMatchesData);
            await CreateAnnouncementViews(Parameter.AnnouncementsData);
            await base.ShowAsync();
        }

        private async UniTask CreateLeagueViews(List<LeagueInfoView.Data> leagueData)
        {
            foreach (var leagueInfo in leagueData)
            {
                var leagueInfoView = Instantiate(leagueInfoViewPrefab, joinedLeaguesParent);
                await leagueInfoView.InitAsync(leagueInfo);
            }
        }

        private async UniTask CreateMatchViews(List<MatchInfoView.Data> matchData)
        {
            foreach (var matchInfo in matchData)
            {
                var matchInfoView = Instantiate(matchInfoViewPrefab, upcomingMatchesParent);
                matchInfoView.InitAsync(matchInfo).Forget();
            }
        }

        private async UniTask CreateAnnouncementViews(List<AnnouncementView.Data> announcementData)
        {
            foreach (var announcementInfo in announcementData)
            {
                // var announcementInfoView = Instantiate(announcementInfoViewPrefab, announcementsParent);
                // announcementInfoView.InitAsync(announcementInfo).Forget();
            }
        }
    }
}