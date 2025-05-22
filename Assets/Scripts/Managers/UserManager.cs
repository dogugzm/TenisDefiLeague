using Cysharp.Threading.Tasks;
using FirebaseService;
using Managers;
using VContainer.Unity;

public class UserManager : IInitializable
{
    public UserData MyData { get; private set; }
    private readonly IFirebaseService _firebaseService;
    private readonly AuthenticationService _authenticationService;

    public UserManager(IFirebaseService firebaseService, AuthenticationService authenticationService)
    {
        _firebaseService = firebaseService;
        _authenticationService = authenticationService;
    }

    public void Initialize()
    {
        _authenticationService.OnAuthenticated += InitUser;
    }

    private void InitUser(UserData data)
    {
        //Sets logged in user
        MyData = data;
    }

    public async UniTask SyncMyData()
    {
        var newData = await GetUserData(MyData.UserID);
        MyData = newData.Value;
    }

    public async UniTask<UserData?> GetUserData(string userId)
    {
        var result = await _firebaseService.GetDataByIdAsync<UserData>(FirebaseCollectionConstants.USERS, userId);
        return result.IsSuccess ? result.Data : null;
    }
    
    
}