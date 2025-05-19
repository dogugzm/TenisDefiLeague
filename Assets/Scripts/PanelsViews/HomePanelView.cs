using System.Collections.Generic;
using Assets.Scripts.PanelService;
using Cysharp.Threading.Tasks;
using Views;

namespace PanelsViews
{
    public class HomePanelView : PanelBase, IInitializableAsync<HomePanelView.Data>
    {
        public class Data
        {
            public List<AnnouncementView.Data> AnnouncementsData { get; }
            public List<LeagueInfoView.Data> JoinedLeaguesData { get; }
            public List<MatchInfoView.Data> UpcomingMatchesData { get; }
        }

        public UniTask InitAsync(Data data)
        {
            return UniTask.CompletedTask;
        }
    }
}