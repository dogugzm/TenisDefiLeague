using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Views
{
    public class LeagueInfoView : MonoBehaviour,IInitializableAsync<LeagueInfoView.Data>
    {
        public class Data
        {
            public string LeagueName { get; }
            public string LeagueDescription { get; }
            public int LeagueParticipants { get; }
            public int UserPosition { get; }
            public LogoView.Data LogoViewData { get; }

            public Data(string leagueName, string leagueDescription, LogoView.Data logoViewData,
                int leagueParticipants, int userPosition)
            {
                LeagueName = leagueName;
                LeagueDescription = leagueDescription;
                LogoViewData = logoViewData;
                LeagueParticipants = leagueParticipants;
                UserPosition = userPosition;
            }
        }

        [SerializeField] private TMP_Text leagueNameText;
        [SerializeField] private TMP_Text leagueDescriptionText;
        [SerializeField] private TMP_Text leagueParticipantsText;
        [SerializeField] private TMP_Text userPositionText;

        [SerializeField] private LogoView logoView;

        public UniTask InitAsync(Data data)
        {
            logoView.InitAsync(data.LogoViewData).Forget();
            leagueNameText.text = data.LeagueName;
            leagueDescriptionText.text = data.LeagueDescription;
            leagueParticipantsText.text = data.LeagueParticipants.ToString();
            userPositionText.text = data.UserPosition.ToString();
            return UniTask.CompletedTask;
        }
    }
}