using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using ListViews;
using PanelService;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace PanelsViews
{
    public class MatchPanelView : PanelBase , ITitleHeader
    {
        [Inject] private MatchService _matchService;
        [Inject] private IObjectResolver _resolver;
        [Inject] private UIThemeSettings _themeSettings;

        [SerializeField] private MatchDataView matchDataViewPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private Image bgImage;

        private List<MatchDataView> _matchDataViews = new();
        [SerializeField] private Transform headerTransform;

        protected override void Awake()
        {
            base.Awake();
            if (bgImage)
            {
                bgImage.color = _themeSettings.PanelBGColor;
            }
        }

        public override async Task ShowAsync()
        {
            var dataList = await _matchService.GetAllMatchesData();
            if (_matchDataViews.Count >= dataList.Count)
            {
                for (var i = 0; i < dataList.Count; i++)
                {
                    _matchDataViews[i].Init(dataList[i]);
                    _matchDataViews[i].gameObject.SetActive(true);
                }

                for (var i = dataList.Count; i < _matchDataViews.Count; i++)
                {
                    _matchDataViews[i].gameObject.SetActive(false);
                }
            }
            else
            {
                for (var i = 0; i < _matchDataViews.Count; i++)
                {
                    _matchDataViews[i].Init(dataList[i]);
                    _matchDataViews[i].gameObject.SetActive(true);
                }

                for (var i = _matchDataViews.Count; i < dataList.Count; i++)
                {
                    var matchDataView = _resolver.Instantiate(matchDataViewPrefab, container);
                    matchDataView.Init(dataList[i]);
                    _matchDataViews.Add(matchDataView);
                }
            }

            await base.ShowAsync();
        }

        public Transform GetHeaderParent() => headerTransform;

        public HeaderPanelViewTitle.Data HeaderData => new("Matches");
        public HeaderPanelViewTitle HeaderView { get; set; }
    }
}