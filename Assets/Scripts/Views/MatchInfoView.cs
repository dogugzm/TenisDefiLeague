using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

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
            public DateTime MatchDate { get; }
            public UserInfoView.Data HomeTeamData { get; }
            public UserInfoView.Data AwayTeamData { get; }
            public List<MatchSetData> MatchScore { get; }
        }

        [SerializeField] private TMP_Text matchDateText;
        [SerializeField] private TMP_Text matchScoreText;
        [SerializeField] private UserInfoView homeTeamLogo;
        [SerializeField] private UserInfoView awayTeamLogo;
        [SerializeField] private TMP_Text leagueNameText;

        public UniTask InitAsync(Data data)
        {
            matchDateText.text = data.MatchDate.ToString("dd/MM/yyyy");
            string matchScore = string.Empty;
            foreach (var setScore in data.MatchScore)
            {
                matchScore += $"{setScore.HomeScore} : {setScore.AwayScore},";
            }

            matchScoreText.text = matchScore;

            return UniTask.CompletedTask;
        }

        public Data Parameter { get; }
    }
}