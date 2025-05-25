using Managers;
using PanelService;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace PanelsViews
{
    public class AuthenticationPanel : PanelBase
    {
        [Inject] private AuthenticationService _authenticationService;
        [Inject] private IPanelService _panelService;

        [SerializeField] Button signInPanelButton;
        [SerializeField] Button signUppanelButton;

        private void OnEnable()
        {
            signInPanelButton.onClick.AddListener(Login);
            signUppanelButton.onClick.AddListener(SignUp);
        }

        private void OnDisable()
        {
            signInPanelButton.onClick.RemoveAllListeners();
            signUppanelButton.onClick.RemoveAllListeners();
        }


        private async void Login()
        {
            await _panelService.HidePanelAsync<RegisterPanelView>();
            _panelService.ShowPanelAsync<LoginPanelView>();
        }

        private async void SignUp()
        {
            await _panelService.HidePanelAsync<LoginPanelView>();
            _panelService.ShowPanelAsync<RegisterPanelView>();
        }
    }
}