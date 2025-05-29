using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseService;
using PanelService;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;
using Views;

namespace PanelsViews
{
    public class LeagueProfilePanel : PanelBase, IPanelParameter<LeagueProfilePanel.Data>
    {
        public class Data
        {
            public LeagueInfoView.Data LeagueInfoData { get; }
            public int Participants { get; }
            public int TotalMatches { get; }

            public List<LeaguePlayerView.Data> LeaguePlayerViews { get; set; }

            public Data(LeagueInfoView.Data leagueInfoData, int participants, int totalMatches,
                List<LeaguePlayerView.Data> leaguePlayerViews)
            {
                LeagueInfoData = leagueInfoData;
                Participants = participants;
                TotalMatches = totalMatches;
                LeaguePlayerViews = leaguePlayerViews;
            }
        }

        [SerializeField] private LeagueInfoView leagueInfoView;
        [SerializeField] private TMP_Text participantText;
        [SerializeField] private TMP_Text playedMatchText;

        [SerializeField] private LeaguePlayerView leaguePlayerViewPrefab;
        [SerializeField] private Transform leaguePlayersContent;

        public Data Parameter { get; set; }
        private IObjectResolver _resolver;
        private List<GameObject> _leaguePlayers = new();

        [Inject]
        private void Injection(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        


        private async Task InitAsync()
        {
            _leaguePlayers.ForEach(x => Destroy(x)); //TODO: Implement pooling
            _leaguePlayers.Clear();

            leagueInfoView.InitAsync(Parameter.LeagueInfoData);
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