using System.Collections.Generic;
using System.Linq;
using Configs;
using FirebaseService;
using PanelService;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Views;

namespace PanelsViews
{
    public class LeaguesPanelView : PanelBase, ITitleHeader
    {
        [SerializeField] LeagueView leaguePrefab;
        [SerializeField] Transform content;
        [SerializeField] private Image bgImage;
        [SerializeField] private Transform headerParent;

        private ILeagueService _leagueService;
        private IFirebaseService _firebaseService;
        private UserManager _userManager;
        private IPanelService _panelService;
        private UIThemeSettings _themeSettings;

        [Inject]
        private void Injection(ILeagueService leagueService, IFirebaseService firebaseService, UserManager userManager,
            IPanelService panelService, UIThemeSettings themeSettings)
        {
            _leagueService = leagueService;
            _firebaseService = firebaseService;
            _userManager = userManager;
            _panelService = panelService;
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
            CreateLeagueRow(league);
        }

        private void CreateLeagueRow(LeagueData data)
        {
            if (data is null) return;

            LeagueView leagueView = Instantiate(leaguePrefab, content);
            leagueView.SetData(new LeagueView.Data {LeagueData = data});

            leagueView.Button.onClick.AddListener(async () =>
            {
                var userList = await _leagueService.GetAllLeaguePlayers(data);
                _panelService.ShowPanelAsync<LeagueProfilePanel, LeagueProfilePanel.Data>(
                    new LeagueProfilePanel.Data(
                        new LeagueView.Data {LeagueData = data},
                        data.Users.Count,
                        data.Users.Count,
                        userList
                    )
                );
            });
        }

        public Transform GetHeaderParent() => headerParent;

        public HeaderPanelViewTitle.Data HeaderData => new("Leagues");
        public HeaderPanelViewTitle HeaderView { get; set; }
    }
}