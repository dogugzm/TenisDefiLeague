using System;
using DG.Tweening;
using Managers;
using PanelService;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace PanelsViews
{
    public class LoginPanelView : PanelBase
    {
        [Inject] private AuthenticationService _authenticationService;

        [FoldoutGroup("Sign In")] [SerializeField]
        private Button signInButton;

        [FoldoutGroup("Sign In")] [SerializeField]
        private TMP_InputField emailInputSignIn;

        [FoldoutGroup("Sign In")] [SerializeField]
        private TMP_InputField passwordInputSignIn;

        [FoldoutGroup("Sign Up")] [SerializeField]
        private Button signUpButton;

        [FoldoutGroup("Sign Up")] [SerializeField]
        private TMP_InputField nameInputSignUp;

        [FoldoutGroup("Sign Up")] [SerializeField]
        private TMP_InputField emailInputSignUp;

        [FoldoutGroup("Sign Up")] [SerializeField]
        private TMP_InputField passwordInputSignUp;

        [FoldoutGroup("UI")] [SerializeField] private Button signInTabButton;
        [FoldoutGroup("UI")] [SerializeField] private Image signInTabHandle;
        [FoldoutGroup("UI")] [SerializeField] private GameObject signInTabContent;
        [FoldoutGroup("UI")] [SerializeField] private Button signUpTabButton;
        [FoldoutGroup("UI")] [SerializeField] private Image signUpTabHandle;
        [FoldoutGroup("UI")] [SerializeField] private GameObject signUpTabContent;

        [FoldoutGroup("UI")] [SerializeField] private Color defaultBGColor;
        [FoldoutGroup("UI")] [SerializeField] private Color activeBGColor;


        private void OnEnable()
        {
            signInButton.onClick.AddListener(SignIn);
            signUpButton.onClick.AddListener(SignUp);
            signInTabButton.onClick.AddListener(SignInTabClicked);
            signUpTabButton.onClick.AddListener(SignUpTabClicked);
        }

        private void OnDisable()
        {
            signInButton.onClick.RemoveListener(SignIn);
            signUpButton.onClick.RemoveListener(SignUp);
            signInTabButton.onClick.RemoveListener(SignInTabClicked);
            signUpTabButton.onClick.RemoveListener(SignUpTabClicked);
        }

        private void SignInTabClicked()
        {
            DOTween.Kill(this);
            signInTabButton.targetGraphic.DOColor(activeBGColor, 0.2f).SetId(this);
            signInTabHandle.DOFade(1, 0.2f).SetId(this);
            signInTabContent.SetActive(true);

            signUpTabButton.targetGraphic.DOColor(defaultBGColor, 0.2f).SetId(this);
            signUpTabHandle.DOFade(0, 0.2f).SetId(this);
            signUpTabContent.SetActive(false);
        }

        private void SignUpTabClicked()
        {
            DOTween.Kill(this);
            signInTabButton.targetGraphic.DOColor(defaultBGColor, 0.2f).SetId(this);
            signInTabHandle.DOFade(0, 0.2f).SetId(this);
            signInTabContent.SetActive(false);

            signUpTabButton.targetGraphic.DOColor(activeBGColor, 0.2f).SetId(this);
            signUpTabHandle.DOFade(1, 0.2f).SetId(this);
            signUpTabContent.SetActive(true);
        }

        private void SignIn()
        {
            if (string.IsNullOrEmpty(emailInputSignIn.text) && string.IsNullOrEmpty(passwordInputSignIn.text)) return;
            _authenticationService.Login(emailInputSignIn.text, passwordInputSignIn.text);
        }

        private void SignUp()
        {
            if (string.IsNullOrEmpty(nameInputSignUp.text) || string.IsNullOrEmpty(emailInputSignUp.text) ||
                string.IsNullOrEmpty(passwordInputSignUp.text)) return;
            _authenticationService.SignUp(emailInputSignUp.text, passwordInputSignUp.text, nameInputSignUp.text);
        }
    }
}