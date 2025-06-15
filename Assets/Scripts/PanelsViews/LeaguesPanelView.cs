using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FirebaseService;
using MockDataSystem;
using PanelService;
using UnityEngine;
using VContainer;
using Views;

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
                if (leagueData.Id == _userManager.MyData.LeaguesJoined) continue;
                CreateLeagueRow(leagueData);
            }
        }

        private void CheckUserLeague(List<LeagueData> leaguesData)
        {
            LeagueData league = leaguesData.FirstOrDefault(item => item.Id == _userManager.MyData.LeaguesJoined);
            if (league == null) return;
            {
            }
            CreateLeagueRow(league);
        }

        private void CreateLeagueRow(LeagueData data)
        {
            if (data is null) return;

            LeagueItem leagueItem = Instantiate(leaguePrefab, content);
            leagueItem.SetData(data);

            leagueItem.Button.onClick.AddListener(async () =>
            {
                var userList = await _leagueService.GetAllLeaguePlayers(data);
                _panelService.ShowPanelAsync<LeagueProfilePanel, LeagueProfilePanel.Data>(
                    new LeagueProfilePanel.Data(
                        new LeagueInfoView.Data(data.Name, data.Description, null, data.Users.Count, data.Users.Count,
                            data.Id),
                        data.Users.Count,
                        data.Users.Count,
                        userList
                    )
                );
            });
        }
    }
}