using Configs;
using PanelService;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Views;

namespace PanelsViews
{
    public class HeaderPanelViewUser : HeaderPanelBase<HeaderPanelViewUser.Data>
    {
        public class Data : HeaderData
        {
            public Data(string panelName, UserInfoView.Data userInfoData) : base(panelName)
            {
                UserInfoData = userInfoData;
            }

            public UserInfoView.Data UserInfoData { get; }
        }

        [SerializeField] private UserInfoView userInfoView;
        [SerializeField] private Image bgImage;
        [SerializeField] private Image safeAreaImage;

        
        [Inject] private UIThemeSettings _themeSettings;
        
        protected override void Awake()
        {
            base.Awake();
            if (bgImage)
            {
                bgImage.color = _themeSettings.HeaderBGColor;
            }
            if (safeAreaImage)
            {
                safeAreaImage.color = _themeSettings.HeaderBGColor;
            }
        }

        public override void Init(Data data)
        {
            if (data == null)
            {
                Debug.LogError("HeaderPanelViewUser: Data is null");
                return;
            }

            if (userInfoView == null)
            {
                Debug.LogError("HeaderPanelViewUser: userInfoView is not assigned");
                return;
            }

            userInfoView.InitAsync(data.UserInfoData);
        }
    }
}