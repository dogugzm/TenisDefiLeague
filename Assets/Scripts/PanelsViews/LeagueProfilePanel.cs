using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.PanelService;
using FirebaseService;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace PanelsViews
{
    public class LeagueProfilePanel : InnerPanelBase<LeaguesPanelView>, IPanelParameter<LeagueProfilePanel.Data>
    {
        public class Data
        {
            public readonly string leagueId;

            public Data(string leagueId)
            {
                this.leagueId = leagueId;
            }
        }

        public Data Parameter { get; set; }
        private IFirebaseService _firebaseService;
        private IObjectResolver _resolver;

        [SerializeField] private TMP_Text leagueName;
        [SerializeField] private TMP_Text leagueDescription;
        [SerializeField] private LeaguePlayerItem leaguePlayerView;
        [SerializeField] private Transform content;

        private List<GameObject> _leaguePlayers = new();

        [Inject]
        private void Injection(IFirebaseService firebaseService, IObjectResolver resolver)
        {
            _firebaseService = firebaseService;
            _resolver = resolver;
        }

        private async void Init(LeagueData data)
        {
            leagueName.text = data.Name;
            leagueDescription.text = data.Description;
            _leaguePlayers.ForEach(x => Destroy(x)); //TODO: Implement pooling
            _leaguePlayers.Clear();

            foreach (var user in data.Users)
            {
                var userData =
                    await _firebaseService.GetDataByIdAsync<UserData>(FirebaseCollectionConstants.USERS, user);
                if (!userData.IsSuccess) continue;

                LeaguePlayerItem leaguePlayer = _resolver.Instantiate(leaguePlayerView, content);
                _leaguePlayers.Add(leaguePlayer.gameObject);
                leaguePlayer.SetData(userData.Data);
            }
        }

        public override async Task ShowAsync()
        {
            var leagueData = await _firebaseService.GetDataByIdAsync<LeagueData>(FirebaseCollectionConstants.LEAGUES,
                Parameter.leagueId);
            if (!leagueData.IsSuccess) return;
            Init(leagueData.Data);
            await base.ShowAsync();
        }
    }
}