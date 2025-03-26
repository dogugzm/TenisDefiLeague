using Assets.Scripts.PanelService;
using FirebaseService;
using TMPro;
using VContainer;

namespace PanelsViews
{
    public class ProfilePanelView : PanelBase
    {
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Rank;

        [Inject] private readonly IFirebaseService _firebaseService;
        [Inject] private readonly UserManager _userManager;

        private void Start()
        {
            InitializePanel();
        }

        private async void InitializePanel()
        {
            var user = await _firebaseService.GetDataByIdAsync<UserData>("Users", _userManager.Data.UserID);
            SetData(user.Data);
        }

        private async void SetData(UserData data)
        {
            Name.text = data.Name;
            var leagueData =
                await _firebaseService.GetDataByIdAsync<LeagueData>(FirebaseCollectionConstants.LEAGUES,
                    "League001");
            //Rank.text = $"#{leagueData.Users.First(item => item.Id == UserManager.Instance.data.UserID).Rank}";
        }
    }
}