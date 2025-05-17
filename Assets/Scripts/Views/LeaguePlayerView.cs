using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Views
{
    public class LeaguePlayerView : MonoBehaviour,IInitializableAsync<LeaguePlayerView.Data>
    {
        public enum Status
        {
            NotAvailable = 0,
            Available = 1,
            InGame = 2,
            Protected = 3,
            Banned = 4,
        }

        public class Data
        {
            public LogoView.Data LogoViewData { get; }
            public string PlayerID { get; }
            public string LeagueID { get; }

            public Data(LogoView.Data logoViewData, string playerID, string leagueID)
            {
                LogoViewData = logoViewData;
                PlayerID = playerID;
                LeagueID = leagueID;
            }
        }

        [SerializeField] private LogoView logoView;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text playerRankText;
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private TMP_Text winLoseText;
        [SerializeField] private TMP_Text statusText;

        public UniTask InitAsync(Data data)
        {
            logoView.InitAsync(data.LogoViewData).Forget();

            SetPlayerInfo(data.PlayerID, data.LeagueID);

            return UniTask.CompletedTask;
        }

        private void SetPlayerInfo(string dataPlayerID, string dataLeagueID)
        {
        }
    }
}