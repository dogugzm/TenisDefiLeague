using System;
using Managers;
using PanelService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace PanelsViews
{
    public class RegisterPanelView : PanelBase
    {
        [Inject] private AuthenticationService _authenticationService;

        [SerializeField] TMP_InputField nameInput;
        [SerializeField] TMP_InputField emailInput;
        [SerializeField] TMP_InputField passwordInput;

        [SerializeField] private Button registerButton;

        private void OnEnable()
        {
            registerButton.onClick.AddListener(SignUp);
        }

        private void OnDisable()
        {
            registerButton.onClick.RemoveListener(SignUp);
        }

        private void SignUp()
        {
            if (string.IsNullOrEmpty(emailInput.text) && string.IsNullOrEmpty(passwordInput.text)) return;
            _authenticationService.SignUp(emailInput.text, passwordInput.text, nameInput.text);
        }
    }
}