using System;
using PanelService;
using PanelsViews;
using UnityEngine;
using VContainer;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject AuthRelatedGO;
        [Inject] IPanelService _panelService;
        [Inject] AuthenticationService _authenticationService;

        private void Awake()
        {
            FirebaseInitializer.FirebaseReady += OpenAuthPanel;
            _authenticationService.OnAuthenticated += OpenHomePanel;
        }

        private void OnDestroy()
        {
            FirebaseInitializer.FirebaseReady -= OpenAuthPanel;
            _authenticationService.OnAuthenticated -= OpenHomePanel;
        }

        private async void OpenAuthPanel()
        {
            if (_authenticationService.HasAuthCache())
            {
                _authenticationService.CacheLogin();
            }
            else
            {
                _panelService.ShowPanelAsync<LoginPanelView>();
            }
        }

        private void OpenHomePanel(UserData data)
        {
            AuthRelatedGO.SetActive(true);
            _panelService.ShowPanelAsync<NavbarPanel>();
        }
    }
}