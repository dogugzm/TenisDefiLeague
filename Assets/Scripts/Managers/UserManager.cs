using Cysharp.Threading.Tasks;
using FirebaseService;
using Managers;
using VContainer.Unity;

public class UserManager : IInitializable
{
    public UserData Data { get; private set; }
    private readonly IFirebaseService _firebaseService;
    private readonly AuthenticationService _authenticationService;


    public UserManager(IFirebaseService firebaseService, AuthenticationService authenticationService)
    {
        _firebaseService = firebaseService;
        _authenticationService = authenticationService;
    }

    public void InitUser(UserData data)
    {
        Data = data;
    }

    public async UniTask SyncUserData()
    {
        var newData =
            await _firebaseService.GetDataByIdAsync<UserData>(FirebaseCollectionConstants.USERS, Data.UserID);
        Data = newData.Data;
    }

    public void Initialize()
    {
        _authenticationService.OnAuthenticated += InitUser;
    }
}