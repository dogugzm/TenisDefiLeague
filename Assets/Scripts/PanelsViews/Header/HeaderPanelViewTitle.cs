using TMPro;
using UnityEngine;

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
        public bool IgnoreJoinStack => false;

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