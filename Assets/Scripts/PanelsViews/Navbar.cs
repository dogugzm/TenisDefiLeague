using Cysharp.Threading.Tasks;
using PanelService;
using VContainer;
using Views;

namespace PanelsViews
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Threading.Tasks;

    public class NavbarPanel : PanelBase, IPersistantPanel
    {
        [SerializeField] private List<Button> tabButtons;
        [Inject] private IPanelService _panelService;

        private IPanel _currentTabPanel;
        private ILeagueService _leagueService;
        private UserManager _userManager;
        private MatchService _matchService;

        [Inject]
        private void Injection(ILeagueService leagueService, UserManager userManager, MatchService matchService)
        {
            _leagueService = leagueService;
            _userManager = userManager;
            _matchService = matchService;
        }

        public override void Initialize()
        {
            base.Initialize();
            SetupTabButtons();
            HomePanelClicked().Forget();
        }

        private void SetupTabButtons()
        {
            if (tabButtons.Count >= 4)
            {
                tabButtons[0].onClick.AddListener(() => HomePanelClicked());
                tabButtons[1].onClick.AddListener(() => SwitchTab<LeaguesPanelView>());
                tabButtons[2].onClick.AddListener(() => SwitchTab<MatchPanelView>());
                tabButtons[3].onClick.AddListener(ProfileButtonClicked);
            }
        }

        private async void ProfileButtonClicked()
        {
            var matches = await _matchService.GetMatchesByUserId(_userManager.MyData.UserID);
            var matchInfoData = new List<MatchInfoView.Data>();
            foreach (var match in matches)
            {
                var homeUser = await _userManager.GetUserData(match.HomeUser);
                var awayUser = await _userManager.GetUserData(match.AwayUser);

                var setDataList = new List<MatchInfoView.MatchSetData>();

                foreach (var setData in match.Sets)
                {
                    setDataList.Add(new MatchInfoView.MatchSetData(setData.HomeUserGames, setData.AwayUserGames));
                }

                matchInfoData.Add(new MatchInfoView.Data(
                    match.EndDate,
                    new UserInfoView.Data(homeUser.Value.Name, null),
                    new UserInfoView.Data(awayUser.Value.Name, null),
                    setDataList
                ));
            }


            var profilePanelData = new ProfilePanelView.Data(
                new UserInfoView.Data(_userManager.MyData.Name, null),
                50,
                _userManager.MyData.MatchPlayed?.Count ?? 0,
                3,
                null,
                matchInfoData
            );

            SwitchTab<ProfilePanelView, ProfilePanelView.Data>(profilePanelData);
        }

        private async UniTask SwitchTab<T>() where T : IPanel
        {
            _currentTabPanel = await _panelService.ShowPanelAsync<T>();
        }

        private async UniTask SwitchTab<T, TE>(TE data) where T : IPanel
        {
            _currentTabPanel = await _panelService.ShowPanelAsync<T, TE>(data);
        }


        public async UniTask HomePanelClicked()
        {
            var leagueData = await _leagueService.TryGetCurrentLeague();
            var joinedLeagueData = new List<LeagueInfoView.Data>();
            int pos;
            if (leagueData is not null)
            {
                pos = await _leagueService.TryGetMyPositionInLeague(leagueData.Id);
                joinedLeagueData = new List<LeagueInfoView.Data>()
                {
                    new(leagueData.Name, leagueData.Description,
                        null, leagueData.Users.Count, pos, leagueData.Id
                    )
                };
            }

            await SwitchTab<HomePanelView, HomePanelView.Data>(new HomePanelView.Data(
                new UserInfoView.Data($"Hey, {_userManager.MyData.Name}", null),
                new List<AnnouncementView.Data>(),
                joinedLeagueData,
                new List<MatchInfoView.Data>()
            ));
        }
    }
}