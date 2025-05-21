using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FirebaseService;
using Sirenix.Utilities;
using UnityEngine;

[FirestoreData]
public struct LeagueData
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public string Description { get; set; }
    [FirestoreProperty] public List<string> Users { get; set; }
    [FirestoreProperty] public DateTime StartDate { get; set; }
    [FirestoreProperty] public DateTime EndDate { get; set; }
}

public interface ILeagueService
{
    UniTask CreateLeague(LeagueData data);
    UniTask DeleteLeague(string leagueID);
    UniTask JoinLeague(string leagueID);
    UniTask LeaveLeague(string leagueID);
    UniTask<LeagueData?> TryGetCurrentLeague();
    UniTask<int> TryGetMyPositionInLeague(string leagueID);
}

public class LeagueService : ILeagueService
{
    private readonly IFirebaseService _firebaseService;
    private readonly UserManager _userManager;

    public LeagueService(IFirebaseService firebaseService, UserManager userManager)
    {
        _firebaseService = firebaseService;
        _userManager = userManager;
    }

    public async UniTask CreateLeague(LeagueData data)
    {
        await _firebaseService.SetDataAsync(data, FirebaseCollectionConstants.LEAGUES, data.Id);
    }

    public UniTask DeleteLeague(string leagueID)
    {
        throw new NotImplementedException();
    }

    async UniTask ILeagueService.JoinLeague(string leagueID)
    {
        if (!string.IsNullOrEmpty(_userManager.Data.LeaguesJoined))
        {
            Debug.LogError("User already joined a league");
            return;
        }

        var addUserTask = _firebaseService.UpdateDataAsync(FirebaseCollectionConstants.LEAGUES, leagueID,
            new Dictionary<string, object>
                { { FirebaseCollectionConstants.USERS, FieldValue.ArrayUnion(_userManager.Data.UserID) } });

        var updateUserData = _firebaseService.UpdateDataAsync(
            newData: new Dictionary<string, object>
                { { "LeaguesJoined", leagueID }, { "UserStatus", UserStatus.AVAILABLE } },
            collectionName: FirebaseCollectionConstants.USERS,
            id: _userManager.Data.UserID);


        var tasks = new UniTask[] { addUserTask, updateUserData };
        await UniTask.WhenAll(tasks);
        await _userManager.SyncUserData();
    }

    public UniTask LeaveLeague(string leagueID)
    {
        throw new NotImplementedException();
    }

    public async UniTask<LeagueData?> TryGetCurrentLeague()
    {
        if (string.IsNullOrEmpty(_userManager.Data.LeaguesJoined))
        {
            return null;
        }

        var data = await _firebaseService.GetDataByIdAsync<LeagueData>(FirebaseCollectionConstants.LEAGUES,
            _userManager.Data.LeaguesJoined);
        if (data.IsSuccess)
        {
            return data.Data;
        }

        return null;
    }

    public async UniTask<int> TryGetMyPositionInLeague(string leagueID)
    {
        var leagueData = await TryGetCurrentLeague();
        if (leagueData == null)
        {
            return -1;
        }

        var users = leagueData.Value.Users;
        if (users.IsNullOrEmpty())
        {
            return -1;
        }

        return users.IndexOf(_userManager.Data.UserID);
    }
}