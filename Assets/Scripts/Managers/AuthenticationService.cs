using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using FirebaseService;
using UnityEngine;

namespace Managers
{
    public class AuthenticationService
    {
        private const string MailKey = "Email";
        private const string PasswordKey = "Password";

        private readonly IFirebaseService _firebaseService;

        public AuthenticationService(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public event Action<UserData> OnAuthenticated;

        private void AuthenticateUser(UserData data)
        {
            OnAuthenticated?.Invoke(data);
        }


        public bool HasAuthCache()
        {
            return PlayerPrefs.HasKey(MailKey) && PlayerPrefs.HasKey(PasswordKey);
        }

        public void CacheLogin()
        {
            FirebaseAuth.DefaultInstance
                .SignInWithEmailAndPasswordAsync(PlayerPrefs.GetString(MailKey), PlayerPrefs.GetString(PasswordKey))
                .ContinueWithOnMainThread(async task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Debug.Log(
                        $"User signed in successfully:,{task.Result.User.DisplayName} ,{task.Result.User.UserId} ");

                    var user = await _firebaseService.GetDataByIdAsync<UserData>(
                        FirebaseCollectionConstants.USERS, task.Result.User.UserId);
                    AuthenticateUser(user.Data);
                });
        }

        public void Login(string mail, string password)
        {
            FirebaseAuth.DefaultInstance
                .SignInWithEmailAndPasswordAsync(mail, password)
                .ContinueWithOnMainThread(async task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Debug.Log(
                        $"User signed in successfully:,{task.Result.User.DisplayName} ,{task.Result.User.UserId} ");

                    var user = await _firebaseService.GetDataByIdAsync<UserData>(
                        FirebaseCollectionConstants.USERS, task.Result.User.UserId);
                    AuthenticateUser(user.Data);

                    PlayerPrefs.SetString(MailKey, mail);
                    PlayerPrefs.SetString(PasswordKey, password);
                });
        }


        public void SignUp(string mail, string password, string name)
        {
            FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(mail, password)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.LogError(
                            "CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Debug.Log(
                        $"Firebase user created successfully: ,{task.Result.User.DisplayName} ,{task.Result.User.UserId} ");

                    task.Result.User.UpdateUserProfileAsync(new UserProfile { DisplayName = name });

                    var newUser = new UserData()
                    {
                        Name = name,
                        UserID = task.Result.User.UserId,
                        Email = task.Result.User.Email,
                        LeaguesJoined = null,
                        MatchPlayed = null,
                        LastMatchesStatus = null,
                        UserStatus = (int)UserStatus.NEWBIE
                    };

                    _firebaseService.SetDataAsync(newUser, FirebaseCollectionConstants.USERS,
                        task.Result.User.UserId);

                    AuthenticateUser(newUser);


                    PlayerPrefs.SetString(MailKey, mail);
                    PlayerPrefs.SetString(PasswordKey, password);
                });
        }
    }
}