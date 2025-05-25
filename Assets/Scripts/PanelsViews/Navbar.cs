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

    public class NavbarPanel : PanelBase
    {
        [SerializeField] private List<Button> tabButtons;
        [Inject] private IPanelService _panelService;

        private IPanel _currentTabPanel;
        private ILeagueService _leagueService;
        private UserManager _userManager;

        [Inject]
        private void Injection(ILeagueService leagueService, UserManager userManager)
        {
            _userManager = userManager;
            _leagueService = leagueService;
        }

        public override void Initialize()
        {
            base.Initialize();
            SetupTabButtons();
        }

        private void SetupTabButtons()
        {
            if (tabButtons.Count >= 4)
            {
                tabButtons[0].onClick.AddListener(() => SwitchTab<HomePanelView>());
                tabButtons[1].onClick.AddListener(() => SwitchTab<LeaguesPanelView>());
                tabButtons[2].onClick.AddListener(() => SwitchTab<MatchPanelView>());
                tabButtons[3].onClick.AddListener(() => SwitchTab<ProfilePanelView>());
            }
        }

        private async UniTask SwitchTab<T>() where T : IPanel
        {
            if (_currentTabPanel != null)
            {
                await _panelService.HidePanelAsync(_currentTabPanel);
            }

            _currentTabPanel = await _panelService.ShowPanelAsync<T>();
        }

        private async UniTask SwitchTab<T, TE>(TE data) where T : IPanel
        {
            if (_currentTabPanel != null)
            {
                await _panelService.HidePanelAsync(_currentTabPanel);
            }

            _currentTabPanel = await _panelService.ShowPanelAsync<T, TE>(data);
        }


        public override async Task ShowAsync()
        {
            await base.ShowAsync();

            var leagueData = await _leagueService.TryGetCurrentLeague();
            var pos = await _leagueService.TryGetMyPositionInLeague(leagueData.Value.Id);

            await SwitchTab<HomePanelView, HomePanelView.Data>(new HomePanelView.Data(
                new UserInfoView.Data($"Hey, {_userManager.MyData.Name}", null),
                new List<AnnouncementView.Data>(),
                new List<LeagueInfoView.Data>()
                {
                    new LeagueInfoView.Data(leagueData.Value.Name, leagueData.Value.Description,
                        null, leagueData.Value.Users.Count, pos, leagueData.Value.Id
                    )
                },
                new List<MatchInfoView.Data>()
            ));
        }

        public override async Task HideAsync()
        {
            if (_currentTabPanel != null)
            {
                await _panelService.HidePanelAsync(_currentTabPanel);
            }

            await base.HideAsync();
        }
    }
}