using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    interface IInitializableAsync<in T>
    {
        public UniTask InitAsync(T data);
    }

    public class LogoView : MonoBehaviour, IInitializableAsync<LogoView.Data>
    {
        public class Data
        {
            public string ImageLink { get; }

            public Data(string imageLink)
            {
                ImageLink = imageLink;
            }
        }

        [SerializeField] private Image logoImage;

        public UniTask InitAsync(Data data)
        {
            logoImage.sprite = GetImageViaLink();
            return UniTask.CompletedTask;
        }

        private Sprite GetImageViaLink()
        {
            return null;
        }
    }
}