using Firebase.Firestore;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FirebaseService;
using VContainer;
using VContainer.Unity;

public enum MatchStatus
{
    NOT_STARTED = 0,
    IN_PROGRESS = 1,
    COMPLETED = 2
}

[FirestoreData]
public class MatchData
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string HomeUser { get; set; }
    [FirestoreProperty] public string AwayUser { get; set; }
    [FirestoreProperty] public DateTime StartDate { get; set; }
    [FirestoreProperty] public DateTime EndDate { get; set; }
    [FirestoreProperty] public List<MatchSetData> Sets { get; set; } = new();
    [FirestoreProperty] public int Status { get; set; } = (int)MatchStatus.NOT_STARTED;
    [FirestoreProperty] public string WinnerUser { get; set; }
}

[FirestoreData]
public class MatchSetData
{
    [FirestoreProperty] public int HomeUserGames { get; set; } = 0;
    [FirestoreProperty] public int AwayUserGames { get; set; } = 0;
}

public class MatchService : IInitializable
{
    private readonly IFirebaseService _firebaseService;
    private readonly UserManager _userManager;

    private readonly List<string> _matchesCollection = new();

    public MatchService(IFirebaseService firebaseService, UserManager userManager)
    {
        _firebaseService = firebaseService;
        _userManager = userManager;
    }

    public void Initialize()
    {
        GetMatches().Forget();
    }

    private async UniTask GetMatches()
    {
        var matches = await _firebaseService.GetDataFromCollectionAsync<MatchData>(FirebaseCollectionConstants.MATCHES);
        if (!matches.IsSuccess) return;
        foreach (var matchData in matches.Data)
        {
            _matchesCollection.Add(matchData.Id);
        }
    }

    public async UniTask<MatchData> TryGetMatchData(string id)
    {
        var matchData = await _firebaseService.GetDataByIdAsync<MatchData>(FirebaseCollectionConstants.MATCHES, id);
        return !matchData.IsSuccess ? null : matchData.Data;
    }

    public async UniTask<List<MatchData>> GetAllMatchesData()
    {
        var matchesData = new List<MatchData>();
        foreach (var matchId in _matchesCollection)
        {
            var matchData = await TryGetMatchData(matchId);
            if (matchData != null) matchesData.Add(matchData);
        }

        return matchesData;
    }

    public async UniTask<bool> TryOfferMatch(string opponentId)
    {
        var opponentUser =
            await _firebaseService.GetDataByIdAsync<UserData>(FirebaseCollectionConstants.USERS, opponentId);
        if (!opponentUser.IsSuccess) return false;
        if (opponentUser.Data.UserStatus != (int)UserStatus.AVAILABLE ||
            _userManager.Data.UserStatus != (int)UserStatus.AVAILABLE) return false;

        var matchData = new MatchData()
        {
            Id = $"Match_{_userManager.Data.Name}_{opponentUser.Data.Name}_{DateTime.UtcNow}",
            HomeUser = _userManager.Data.UserID,
            AwayUser = opponentId,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow + TimeSpan.FromDays(10),
            Sets = null,
            Status = (int)MatchStatus.IN_PROGRESS,
        };

        if (!await TryCreateMatch(matchData)) return false;

        await SetUsersStatus(opponentId);
        _matchesCollection.Add(matchData.Id);
        return true;
    }

    public async UniTask CompleteMatch(string matchId, List<MatchSetData> sets)
    {
        var newMatchData = await _firebaseService.UpdateDataAsync(FirebaseCollectionConstants.MATCHES, matchId,
            new Dictionary<string, object>()
            {
                { nameof(MatchData.Status), MatchStatus.COMPLETED },
                {
                    nameof(MatchData.Sets), new List<MatchSetData>()
                    {
                        new()
                            { HomeUserGames = 6, AwayUserGames = 4, },
                        new()
                            { HomeUserGames = 6, AwayUserGames = 4, },
                        new()
                            { HomeUserGames = 6, AwayUserGames = 4, },
                    }
                } /*,
                { nameof(MatchData.Sets), sets }*/
            });
    }

    private async UniTask<bool> TryCreateMatch(MatchData data)
    {
        var createdMatchData =
            await _firebaseService.SetDataAsync(data, FirebaseCollectionConstants.MATCHES, data.Id);
        return createdMatchData.IsSuccess;
    }

    private async UniTask SetUsersStatus(string opponentId)
    {
        await _firebaseService.UpdateDataAsync(FirebaseCollectionConstants.USERS, opponentId,
            new Dictionary<string, object>()
            {
                { nameof(UserData.UserStatus), UserStatus.IN_MATCH },
            });

        await _firebaseService.UpdateDataAsync(FirebaseCollectionConstants.USERS, _userManager.Data.UserID,
            new Dictionary<string, object>()
            {
                { nameof(UserData.UserStatus), UserStatus.IN_MATCH },
            });

        await _userManager.SyncUserData();
    }
}