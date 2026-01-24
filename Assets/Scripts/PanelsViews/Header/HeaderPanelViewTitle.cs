using Configs;
using PanelService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace PanelsViews
{
    public class HeaderPanelViewTitle : HeaderPanelBase<HeaderPanelViewTitle.Data>, IIgnoreJoinStack
    {
        public class Data : HeaderData
        {
            public Data(string panelName) : base(panelName)
            {
            }
        }

        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Image bgImage;
        [SerializeField] private Image safeAreaImage;

        
        [Inject] private UIThemeSettings _themeSettings;
        
        public bool IgnoreJoinStack => false;

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
                Debug.LogError("HeaderPanelViewTitle: Data is null");
                return;
            }

            if (titleText == null)
            {
                Debug.LogError("HeaderPanelViewTitle: titleText is not assigned");
                return;
            }

            titleText.text = data.PanelName;
        }
    }
}