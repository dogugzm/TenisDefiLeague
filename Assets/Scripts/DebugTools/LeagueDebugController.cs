using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace DebugTools
{
    public class LeagueDebugController : MonoBehaviour
    {
        [Inject] private readonly ILeagueService _leagueService;
        [Inject] private readonly UserManager _userManager;

        private void Start()
        {
            if (_leagueService == null)
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

        [Title("League Creation")]
        [SerializeField] private string _leagueId;
        [SerializeField] private string _leagueName = "Test League";
        [SerializeField] private string _leagueDescription = "This is a test league created from Unity Editor";
        [SerializeField] private DateTime _startDate = DateTime.Now;
        [SerializeField] private DateTime _endDate = DateTime.Now.AddDays(7);

        [Title("Debug Operations")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void GenerateNewId()
        {
            _leagueId = Guid.NewGuid().ToString();
        }

        [Button(ButtonSizes.Large)]
        public async void CreateLeague()
        {
            if (_leagueService == null)
            {
                Log("League Service is not injected! Make sure you are in Play Mode and VContainer is set up.");
                return;
            }

            if (string.IsNullOrEmpty(_leagueId))
            {
                GenerateNewId();
            }

            var data = new LeagueData
            {
                Id = _leagueId,
                Name = _leagueName,
                Description = _leagueDescription,
                StartDate = _startDate,
                EndDate = _endDate,
                Users = new List<string>() 
            };

            Log($"Creating League: {data.Name} ({data.Id})...");

            try
            {
                await _leagueService.CreateLeague(data);
                Log("League Created Successfully!");
            }
            catch (Exception e)
            {
                Log($"Error creating league: {e.Message}");
                Debug.LogException(e);
            }
        }

        [Title("Existing League Operations")]
        [SerializeField] private string _targetLeagueId;

        [Button]
        public async void JoinLeague()
        {
            if (_leagueService == null) return;

            // Use _targetLeagueId or _leagueId if target is empty
            var id = string.IsNullOrEmpty(_targetLeagueId) ? _leagueId : _targetLeagueId;

            Log($"Joining League: {id}...");
            try
            {
                await _leagueService.JoinLeague(id);
                Log("Joined League Successfully!");
            }
            catch (Exception e)
            {
                Log($"Error joining league: {e.Message}");
                Debug.LogException(e);
            }
        }

        [Button]
        public async void GetLeagueInfo()
        {
            if (_leagueService == null) return;
            var id = string.IsNullOrEmpty(_targetLeagueId) ? _leagueId : _targetLeagueId;

            Log($"Fetching League: {id}...");
            try
            {
                var data = await _leagueService.GetLeague(id);
                if (data != null)
                {
                    Log($"League Found: {data.Name}\nDesc: {data.Description}\nUsers: {data.Users?.Count ?? 0}");
                }
                else
                {
                    Log("League not found.");
                }
            }
            catch (Exception e)
            {
                Log($"Error fetching league: {e.Message}");
                Debug.LogException(e);
            }
        }

        [Title("User Info")]
        [Button]
        public void LogCurrentUser()
        {
            if (_userManager == null)
            {
                Log("User Manager not injected.");
                return;
            }

            if (_userManager.MyData.UserID != null)
            {
                Log($"User: {_userManager.MyData.Name} ({_userManager.MyData.UserID})\nJoined League: {_userManager.MyData.LeaguesJoined}");
            }
            else
            {
                Log("User Data is null (not logged in?)");
            }
        }

        [Title("Output")]
        [TextArea(5, 20)]
        [SerializeField] private string _logOutput;

        private void Log(string message)
        {
            Debug.Log($"[LeagueDebug] {message}");
            _logOutput = $"[{DateTime.Now:HH:mm:ss}] {message}\n" + _logOutput;
        }
    }
}