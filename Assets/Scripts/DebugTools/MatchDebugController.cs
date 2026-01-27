using System;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace DebugTools
{
    public class MatchDebugController : MonoBehaviour
    {
        [Inject] private readonly MatchService _matchService;
        [Inject] private readonly UserManager _userManager;

        private void Start()
        {
            if (_matchService == null)
            {
                var scope = FindObjectOfType<GameLifetimeScope>();
                if (scope != null)
                {
                    if (scope.Container != null)
                    {
                        scope.Container.Inject(this);
                        Log("Manually injected dependencies via GameLifetimeScope.");
                    }
                    else
                    {
                        Log("GameLifetimeScope found but Container is null. Is it initialized?");
                    }
                }
                else
                {
                    Log("GameLifetimeScope not found in scene.");
                }
            }
        }

        [Title("Match Creation")]
        [SerializeField] private string _opponentId;

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        public async void OfferMatch()
        {
            if (_matchService == null || _userManager == null)
            {
                Log("Services not injected! Make sure you are in Play Mode and VContainer is set up.");
                return;
            }

            if (string.IsNullOrEmpty(_opponentId))
            {
                Log("Opponent ID is required to offer a match.");
                return;
            }

            Log($"Offering match to opponent: {_opponentId}...");

            try
            {
                var success = await _matchService.TryOfferMatch(_opponentId);
                if (success)
                    Log("Match Offered/Created Successfully!");
                else
                    Log("Failed to offer match. Check if users are available and ID is correct.");
            }
            catch (Exception e)
            {
                Log($"Error offering match: {e.Message}");
                Debug.LogException(e);
            }
        }

        [Title("Existing Match Operations")]
        [SerializeField] private string _targetMatchId;

        [Title("Match Results Input")]
        [InfoBox("Add sets here to simulate match results when completing a match.")]
        [SerializeField]
        private List<MatchSetData> _matchSets = new List<MatchSetData>
        {
            new MatchSetData { HomeUserGames = 6, AwayUserGames = 0 },
            new MatchSetData { HomeUserGames = 6, AwayUserGames = 0 }
        };

        [Button(ButtonSizes.Large)]
        public async void CompleteMatch()
        {
            if (_matchService == null) return;

            if (string.IsNullOrEmpty(_targetMatchId))
            {
                Log("Target Match ID is required.");
                return;
            }

            Log($"Completing Match: {_targetMatchId}...");
            try
            {
                await _matchService.CompleteMatch(_targetMatchId, _matchSets);
                Log("Match Completed request sent.");
            }
            catch (Exception e)
            {
                Log($"Error completing match: {e.Message}");
                Debug.LogException(e);
            }
        }

        [Button]
        public async void GetMatchInfo()
        {
            if (_matchService == null) return;

            if (string.IsNullOrEmpty(_targetMatchId))
            {
                Log("Target Match ID is required.");
                return;
            }

            Log($"Fetching Match: {_targetMatchId}...");
            try
            {
                var data = await _matchService.TryGetMatchData(_targetMatchId);
                if (data != null)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Match Found: {data.Id}");
                    sb.AppendLine($"Home: {data.HomeUser} vs Away: {data.AwayUser}");
                    sb.AppendLine($"Status: {(MatchStatus)data.Status}");
                    sb.AppendLine($"LeagueID: {data.LeagueID}");
                    sb.AppendLine($"Sets: {data.Sets?.Count ?? 0}");
                    if (data.Sets != null)
                    {
                        for (int i = 0; i < data.Sets.Count; i++)
                        {
                            sb.AppendLine($"  Set {i + 1}: {data.Sets[i].HomeUserGames}-{data.Sets[i].AwayUserGames}");
                        }
                    }
                    Log(sb.ToString());
                }
                else
                {
                    Log("Match not found.");
                }
            }
            catch (Exception e)
            {
                Log($"Error fetching match: {e.Message}");
                Debug.LogException(e);
            }
        }

        [Button]
        public async void ListMyMatches()
        {
            if (_matchService == null || _userManager == null) return;

            if ( string.IsNullOrEmpty(_userManager.MyData.UserID))
            {
                Log("Current User is not logged in or UserData is missing.");
                return;
            }

            Log($"Fetching matches for user: {_userManager.MyData.Name} ({_userManager.MyData.UserID})...");
            try
            {
                var matches = await _matchService.GetMatchesByUserId(_userManager.MyData.UserID);
                Log($"Found {matches.Count} matches.");
                foreach (var m in matches)
                {
                    string opponent = m.HomeUser == _userManager.MyData.UserID ? m.AwayUser : m.HomeUser;
                    Log($"[{m.Id}] Status: {(MatchStatus)m.Status} | vs {opponent}");
                }
            }
            catch (Exception e)
            {
                Log($"Error fetching my matches: {e.Message}");
                Debug.LogException(e);
            }
        }

        [Title("Output")]
        [TextArea(5, 20)]
        [SerializeField] private string _logOutput;

        private void Log(string message)
        {
            Debug.Log($"[MatchDebug] {message}");
            _logOutput = $"[{DateTime.Now:HH:mm:ss}] {message}\n" + _logOutput;
        }
    }
}
