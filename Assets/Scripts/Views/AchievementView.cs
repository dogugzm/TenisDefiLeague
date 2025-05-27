using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Views
{
    public class AchievementView : MonoBehaviour, IInitializableAsync<AchievementView.Data>
    {
        public class Data
        {
        }

        public UniTask InitAsync(Data data)
        {
            Parameter = data;
            throw new System.NotImplementedException();
        }

        public Data Parameter { get; private set; }
    }
}