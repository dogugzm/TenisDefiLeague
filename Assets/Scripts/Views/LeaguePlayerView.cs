using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views
{
    public class LeaguePlayerView : MonoBehaviour, IInitializableAsync<LeaguePlayerView.Data>
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
            public UserInfoView.Data UserViewData { get; }
            public string PlayerID { get; }
            public string LeagueID { get; }

            public Data(UserInfoView.Data userViewData, string playerID, string leagueID)
            {
                UserViewData = userViewData;
                PlayerID = playerID;
                LeagueID = leagueID;
            }
        }

        [SerializeField] private UserInfoView userView;
        [SerializeField] private TMP_Text playerRankText;
        [SerializeField] private TMP_Text rankDiffText;
        [SerializeField] private TMP_Text winLoseText;
        [SerializeField] private TMP_Text statusText;

        public UniTask InitAsync(Data data)
        {
            userView.InitAsync(data.UserViewData).Forget();

            SetPlayerInfo(data.PlayerID, data.LeagueID);

            return UniTask.CompletedTask;
        }

        private void SetPlayerInfo(string dataPlayerID, string dataLeagueID)
        {
        }
    }
}