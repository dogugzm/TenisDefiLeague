
using System;
using System.Collections.Generic;
using Firebase.Firestore;

public enum UserStatus
{
    NEWBIE = 0,
    AVAILABLE = 1,
    UNAVAILABLE = 2,
    IN_PROTECTION = 3,
    IN_MATCH = 4
}

[FirestoreData]
public struct UserData
{
    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public string Email { get; set; }
    [FirestoreProperty] public string UserID { get; set; }
    [FirestoreProperty] public string? LeaguesJoined { get; set; }
    [FirestoreProperty] public List<string>? MatchPlayed { get; set; }
    [FirestoreProperty] public List<bool>? LastMatchesStatus { get; set; }
    [FirestoreProperty] public int UserStatus { get; set; }
}