using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class InputFieldView : MonoBehaviour, IInitializableAsync<InputFieldView.Data>
    {
        public class Data
        {
            public string PlaceholderText { get; }
            public Action OnValueChanged { get; }

            public Data(string placeholderText, Action onValueChanged = null)
            {
                PlaceholderText = placeholderText;
                OnValueChanged = onValueChanged;
            }
        }

        [SerializeField] private TMP_InputField inputField;

        public string GetInputFieldText() => inputField.text;

        public UniTask InitAsync(Data data)
        {
            inputField.placeholder.GetComponent<TMP_Text>().text = data.PlaceholderText;

            inputField.onValueChanged.AddListener(_ => { data.OnValueChanged?.Invoke(); });

            return UniTask.CompletedTask;
        }

        public Data Parameter { get; }
    }
}