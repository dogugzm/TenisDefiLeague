using Cysharp.Threading.Tasks;
using VContainer;

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

        public override async Task ShowAsync()
        {
            await base.ShowAsync();
            // Default olarak ilk tab'i aç
            await SwitchTab<HomePanelView>();
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