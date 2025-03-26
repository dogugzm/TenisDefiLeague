using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.PanelService;
using FirebaseService;
using UnityEngine;
using VContainer;

namespace PanelsViews
{
    public class LeaguesPanelView : PanelBase
    {
        [SerializeField] LeagueItem leaguePrefab;
        [SerializeField] Transform content;

        private ILeagueService _leagueService;
        private IFirebaseService _firebaseService;
        private UserManager _userManager;
        private IPanelService _panelService;

        [Inject]
        private void Injection(ILeagueService leagueService, IFirebaseService firebaseService, UserManager userManager,
            IPanelService panelService)
        {
            _leagueService = leagueService;
            _firebaseService = firebaseService;
            _userManager = userManager;
            _panelService = panelService;
        }

        private async void Start()
        {
            var leagues =
                await _firebaseService.GetDataFromCollectionAsync<LeagueData>
                    (FirebaseCollectionConstants.LEAGUES);

            if (!leagues.IsSuccess) return;

            CheckUserLeague(leagues.Data);

            foreach (var leagueData in leagues.Data)
            {
                if (leagueData.Id == _userManager.Data.LeaguesJoined) continue;
                CreateLeagueRow(leagueData);
            }
        }

        private void CheckUserLeague(List<LeagueData> leaguesData)
        {
            var league = leaguesData.FirstOrDefault(item => item.Id == _userManager.Data.LeaguesJoined);
            CreateLeagueRow(league);
        }

        private void CreateLeagueRow(LeagueData data)
        {
            LeagueItem leagueItem = Instantiate(leaguePrefab, content);
            leagueItem.SetData(data);

            leagueItem.Button.onClick.AddListener(() =>
            {
                _panelService.ShowPanelAsync<LeagueProfilePanel, LeagueProfilePanel.Data>(
                    new LeagueProfilePanel.Data(data.Id));
            });
        }
    }
}