using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Views
{
    public class UserInfoView : MonoBehaviour, IInitializableAsync<UserInfoView.Data>
    {
        public class Data
        {
             public string UserName { get; }
            public LogoView.Data LogoViewData { get; }

            public Data(string userName, LogoView.Data logoViewData)
            {
                UserName = userName;
                LogoViewData = logoViewData;
            }
        }

        [SerializeField] private TMP_Text userNameText;
        [SerializeField] private LogoView logoView;

        public UniTask InitAsync(Data data)
        {
            logoView.InitAsync(data.LogoViewData).Forget();
            userNameText.text = data.UserName;

            return UniTask.CompletedTask;
        }

        public Data Parameter { get; }
    }
}