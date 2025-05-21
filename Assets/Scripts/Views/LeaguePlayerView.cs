using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

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
            public int PlayerRank { get; }
            public int RankDiff { get; }
            public int Wins { get; }
            public int Losses { get; }
            public Status PlayerStatus { get; }

            public Data(UserInfoView.Data userViewData, int playerRank, int rankDiff, int wins, int losses,
                Status playerStatus)
            {
                UserViewData = userViewData;
                PlayerRank = playerRank;
                RankDiff = rankDiff;
                Wins = wins;
                Losses = losses;
                PlayerStatus = playerStatus;
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
            playerRankText.text = data.PlayerRank.ToString();
            rankDiffText.text = data.RankDiff.ToString();
            winLoseText.text = $"{data.Wins} / {data.Losses}";
            statusText.text = data.PlayerStatus.ToString();

            return UniTask.CompletedTask;
        }
    }
}