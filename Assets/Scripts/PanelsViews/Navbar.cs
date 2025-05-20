using Cysharp.Threading.Tasks;
using VContainer;
using Views;

namespace PanelsViews
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Threading.Tasks;
    using Assets.Scripts.PanelService;

    public class NavbarPanel : PanelBase
    {
        [SerializeField] private List<Button> tabButtons;
        [Inject] private IPanelService _panelService;
        private IPanel _currentTabPanel;

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

            await SwitchTab<HomePanelView, HomePanelView.Data>(new HomePanelView.Data(
                new UserInfoView.Data("Hey, Dogukan", new LogoView.Data("dasadsf")),
                new List<AnnouncementView.Data>(),
                new List<LeagueInfoView.Data>(),
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