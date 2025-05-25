using System;
using Managers;
using PanelService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace PanelsViews
{
    public class LoginPanelView : PanelBase
    {
        [Inject] private AuthenticationService _authenticationService;

        [SerializeField] private Button SignInButton;
        [SerializeField] TMP_InputField emailInputSignIn;
        [SerializeField] TMP_InputField passwordInputSignIn;

        private void OnEnable()
        {
            SignInButton.onClick.AddListener(SignIn);
        }

        private void OnDisable()
        {
            SignInButton.onClick.RemoveListener(SignIn);
        }

        private void SignIn()
        {
            if (string.IsNullOrEmpty(emailInputSignIn.text) && string.IsNullOrEmpty(passwordInputSignIn.text)) return;
            _authenticationService.Login(emailInputSignIn.text, passwordInputSignIn.text);
        }
    }
}