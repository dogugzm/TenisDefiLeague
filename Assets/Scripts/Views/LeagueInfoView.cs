using Cysharp.Threading.Tasks;
using PanelService;
using PanelsViews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Views
{
    public class LeagueInfoView : MonoBehaviour, IInitializableAsync<LeagueInfoView.Data>
    {
        public class Data
        {
            public string LeagueName { get; }
            public string LeagueDescription { get; }
            public int LeagueParticipants { get; }
            public int UserPosition { get; }
            public LogoView.Data LogoViewData { get; }
            public string LeagueID { get; set; }

            public Data(string leagueName, string leagueDescription, LogoView.Data logoViewData,
                int leagueParticipants, int userPosition, string leagueID)
            {
                LeagueName = leagueName;
                LeagueDescription = leagueDescription;
                LogoViewData = logoViewData;
                LeagueParticipants = leagueParticipants;
                UserPosition = userPosition;
                LeagueID = leagueID;
            }
        }

        [SerializeField] private TMP_Text leagueNameText;
        [SerializeField] private TMP_Text leagueDescriptionText;
        [SerializeField] private TMP_Text leagueParticipantsText;
        [SerializeField] private TMP_Text userPositionText;

        [SerializeField] private LogoView logoView;
        [SerializeField] private Button leagueButton;

        private IPanelService _panelService;
        private ILeagueService _leagueService;
        public Data Parameter { get; set; }

        [Inject]
        public void Injection(IPanelService panelService, ILeagueService leagueService)
        {
            _leagueService = leagueService;
            _panelService = panelService;
        }


        public async UniTask InitAsync(Data data)
        {
            Parameter = data;
            //await logoView.InitAsync(data.LogoViewData);
            leagueNameText.text = data.LeagueName;
            if (leagueDescriptionText != null) leagueDescriptionText.text = data.LeagueDescription;
            leagueParticipantsText.text = data.LeagueParticipants.ToString();
            userPositionText.text = data.UserPosition.ToString();

            if (leagueButton != null)
            {
                leagueButton.onClick.RemoveAllListeners();
                leagueButton.onClick.AddListener(() => OnLeagueButtonClicked().Forget());
            }
        }

        private async UniTaskVoid OnLeagueButtonClicked()
        {
            var leagueData = await _leagueService.GetLeague(Parameter.LeagueID);
            var userList = await _leagueService.GetAllLeaguePlayers(leagueData.Value);

            var panelData = new LeagueProfilePanel.Data(
                Parameter,
                leagueData.Value.Users.Count,
                leagueData.Value.Users.Count,
                userList
            );
            await _panelService.ShowPanelAsync<LeagueProfilePanel, LeagueProfilePanel.Data>(panelData);
        }
    }
}