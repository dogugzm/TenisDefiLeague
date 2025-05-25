using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ProgressView :MonoBehaviour, IInitializableAsync<ProgressView.Data>
    {
        public class Data
        {
            public string ProgressHeader { get; }
            public float CurrentProgress { get; }
            public float TargetProgress { get; }


            public Data(string progressHeader, float currentProgress, float targetProgress)
            {
                ProgressHeader = progressHeader;
                CurrentProgress = currentProgress;
                TargetProgress = targetProgress;
            }
        }

        [SerializeField] private TextMeshProUGUI progressHeaderText;
        [SerializeField] private TextMeshProUGUI progressText;

        [SerializeField] private Slider progressSlider;

        public UniTask InitAsync(Data data)
        {
            progressHeaderText.text = data.ProgressHeader;
            progressText.text = $"{data.CurrentProgress}/{data.TargetProgress}";
            progressSlider.value = data.CurrentProgress / data.TargetProgress;

            return UniTask.CompletedTask;
        }

        public Data Parameter { get; }
    }
}