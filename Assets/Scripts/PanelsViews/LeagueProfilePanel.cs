using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using PanelService;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using Views;

namespace PanelsViews
{
    public class LeagueProfilePanel : PanelBase, IPanelParameter<LeagueProfilePanel.Data>
    {
        public class Data
        {
            public LeagueView.Data LeagueInfoData { get; }
            public int Participants { get; }
            public int TotalMatches { get; }
            public List<LeaguePlayerView.Data> LeaguePlayerViews { get; set; }

            public Data(LeagueView.Data leagueInfoData, int participants, int totalMatches,
                List<LeaguePlayerView.Data> leaguePlayerViews)
            {
                LeagueInfoData = leagueInfoData;
                Participants = participants;
                TotalMatches = totalMatches;
                LeaguePlayerViews = leaguePlayerViews;
            }
        }

        [SerializeField] private LeagueView leagueView;
        [SerializeField] private TMP_Text participantText;
        [SerializeField] private TMP_Text playedMatchText;

        [SerializeField] private LeaguePlayerView leaguePlayerViewPrefab;
        [SerializeField] private Image bgImage;
        
        [SerializeField] private Transform leaguePlayersContent;

        public Data Parameter { get; set; }
        private IObjectResolver _resolver;
        private UIThemeSettings _themeSettings;
        
        

        private List<GameObject> _leaguePlayers = new();

        [Inject]
        private void Injection(IObjectResolver resolver, UIThemeSettings themeSettings)
        {
            _resolver = resolver;
            _themeSettings = themeSettings;
        }

        protected override void Awake()
        {
            base.Awake();
            if (bgImage)
            {
                bgImage.color = _themeSettings.PanelBGColor;
            }
        }

        private async Task InitAsync()
        {
            _leaguePlayers.ForEach(x => Destroy(x)); //TODO: Implement pooling
            _leaguePlayers.Clear();

            leagueView.SetData(Parameter.LeagueInfoData);
            participantText.text = Parameter.Participants.ToString();
            playedMatchText.text = Parameter.TotalMatches.ToString();
            foreach (var player in Parameter.LeaguePlayerViews)
            {
                var leaguePlayerView = _resolver.Instantiate(leaguePlayerViewPrefab, leaguePlayersContent);
                await leaguePlayerView.InitAsync(player);
                _leaguePlayers.Add(leaguePlayerView.gameObject);
            }
        }
        
        public override async Task ShowAsync()
        {
            InitAsync();
            await base.ShowAsync();
        }
    }
}