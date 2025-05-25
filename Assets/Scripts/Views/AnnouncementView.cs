using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Views
{
    public class AnnouncementView : MonoBehaviour, IInitializableAsync<AnnouncementView.Data>
    {
        public class Data
        {
            public string AnnouncementTitle { get; }
            public string AnnouncementDescription { get; }
            public LogoView.Data LogoViewData { get; }

            public Data(string announcementTitle, string announcementDescription, LogoView.Data logoViewData)
            {
                AnnouncementTitle = announcementTitle;
                AnnouncementDescription = announcementDescription;
                LogoViewData = logoViewData;
            }
        }

        [SerializeField] private TMP_Text announcementTitleText;
        [SerializeField] private TMP_Text announcementDescriptionText;
        [SerializeField] private TMP_Text announcementTimeText;

        [SerializeField] private LogoView logoView;

        public UniTask InitAsync(Data data)
        {
            logoView.InitAsync(data.LogoViewData).Forget();
            announcementTitleText.text = data.AnnouncementTitle;
            announcementDescriptionText.text = data.AnnouncementDescription;
            return UniTask.CompletedTask;
        }

        public Data Parameter { get; }
    }
}