using UnityEngine;
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