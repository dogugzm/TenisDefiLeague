using System;
using System.Collections.Generic;
using Coffee.UIEffects;
using Configs;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using VContainer;

namespace Views
{
    public class MatchInfoView : MonoBehaviour, IInitializableAsync<MatchInfoView.Data>
    {
        public class MatchSetData
        {
            public MatchSetData(int homeScore, int awayScore)
            {
                HomeScore = homeScore;
                AwayScore = awayScore;
            }

            public int HomeScore { get; }
            public int AwayScore { get; }
        }

        public class Data
        {
            public Data(DateTime matchDate, UserInfoView.Data homeTeamData, UserInfoView.Data awayTeamData,
                List<MatchSetData> matchScore, [CanBeNull] string leagueName, bool ısHomeWinner)
            {
                MatchDate = matchDate;
                HomeTeamData = homeTeamData;
                AwayTeamData = awayTeamData;
                MatchScore = matchScore;
                LeagueName = leagueName;
                IsHomeWinner = ısHomeWinner;
            }

            public DateTime MatchDate { get; }
            public UserInfoView.Data HomeTeamData { get; }
            public UserInfoView.Data AwayTeamData { get; }
            public List<MatchSetData> MatchScore { get; }
            [CanBeNull] public string LeagueName { get; }
            public bool IsHomeWinner { get; }
        }

        [Inject] private UIThemeSettings _themeSettings;

        [SerializeField] private TMP_Text matchDateText;
        [SerializeField] private UserInfoView homeTeamLogo;
        [SerializeField] private UserInfoView awayTeamLogo;
        [SerializeField] private TMP_Text leagueNameText;
        [SerializeField] private Transform homeScoreParent;
        [SerializeField] private Transform awayScoreParent;
        [SerializeField] private ScoreItemView scoreItemPrefab;
        [SerializeField] private UIEffect effect;

        public UniTask InitAsync(Data data)
        {
            effect.edgeColor = data.IsHomeWinner ? Color.green : Color.red; //TODO-DG: add to theme settings
            homeTeamLogo.InitAsync(data.HomeTeamData);
            awayTeamLogo.InitAsync(data.AwayTeamData);

            if (data.LeagueName is not null)
            {
                leagueNameText.text = data.LeagueName;
            }

            matchDateText.text = data.MatchDate.ToString("MMM dd, yyyy HH:mm");

            foreach (var sets in data.MatchScore)
            {
                var homeScoreItem = Instantiate(scoreItemPrefab, homeScoreParent);
                homeScoreItem.Init(new ScoreItemView.Data(sets.HomeScore,
                    sets.HomeScore > sets.AwayScore ? _themeSettings.ActiveColor : _themeSettings.DeactiveColor));

                var awayScoreItem = Instantiate(scoreItemPrefab, awayScoreParent);
                awayScoreItem.Init(new ScoreItemView.Data(sets.AwayScore,
                    sets.AwayScore > sets.HomeScore ? _themeSettings.ActiveColor : _themeSettings.DeactiveColor));
            }

            return UniTask.CompletedTask;
        }

        public Data Parameter { get; }
    }
}