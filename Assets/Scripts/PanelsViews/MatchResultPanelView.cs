using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using PanelService;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace PanelsViews
{
    public class MaxSetCountExceededException : Exception
    {
        public MaxSetCountExceededException() : base("Maximum set count exceeded.")
        {
        }
    }

    public class MatchResultPanelView : PanelBase, IPanelParameter<MatchResultPanelView.Data>, ITitleHeader
    {
        public class Data
        {
            public Data(MatchInfoView.Data matchData)
            {
                MatchData = matchData;
            }

            public MatchInfoView.Data MatchData { get; }
        }

        private const int MAX_SET_COUNT = 6;

        [SerializeField] private MatchInfoView matchInfoView;
        [SerializeField] private Button addSetButton;
        [SerializeField] private Button removeSetButton;
        [SerializeField] private Button submitScoreButton;

        [SerializeField] private MatchSetDataView matchSetDataView;
        [SerializeField] private Transform matchSetDataParent;

        [SerializeField] private Transform headerParent;

        private List<MatchSetDataView> _matchSetDataViews = new();

        public Data Parameter { get; set; }
        public Transform GetHeaderParent() => headerParent;

        public HeaderPanelViewTitle.Data HeaderData => new HeaderPanelViewTitle.Data("Score Entry");
        public HeaderPanelViewTitle HeaderView { get; set; }

        private UniTask InitAsync()
        {
            UpdateButtonStates();
            matchInfoView.InitAsync(Parameter.MatchData).Forget();

            addSetButton.onClick.AddListener(OnAddSetButtonClicked);
            removeSetButton.onClick.AddListener(OnRemoveSetButtonClicked);

            OnAddSetButtonClicked();

            return UniTask.CompletedTask;
        }

        public override async Task ShowAsync()
        {
            await InitAsync();
            await base.ShowAsync();
        }

        public override Task HideAsync()
        {
            addSetButton.onClick.RemoveListener(OnAddSetButtonClicked);
            removeSetButton.onClick.RemoveListener(OnRemoveSetButtonClicked);
            return base.HideAsync();
        }

        private void OnAddSetButtonClicked()
        {
            try
            {
                UpdateButtonStates();
                if (_matchSetDataViews.Count >= MAX_SET_COUNT) throw new MaxSetCountExceededException();

                var matchSetDataViewInstance = Instantiate(matchSetDataView, matchSetDataParent);
                _matchSetDataViews.Add(matchSetDataViewInstance);

                addSetButton.transform.parent.SetAsLastSibling();
            }
            catch (MaxSetCountExceededException e)
            {
                Debug.LogException(e);
            }
        }

        private void UpdateButtonStates()
        {
            addSetButton.interactable = _matchSetDataViews.Count < MAX_SET_COUNT;
            removeSetButton.interactable = _matchSetDataViews.Count > 0;
        }

        private void OnRemoveSetButtonClicked()
        {
            UpdateButtonStates();
            if (_matchSetDataViews.Count <= 0) return;
            var lastSetDataView = _matchSetDataViews[^1];
            _matchSetDataViews.Remove(lastSetDataView);
            Destroy(lastSetDataView.gameObject);
        }
    }
}