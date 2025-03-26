using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.PanelService;
using Cysharp.Threading.Tasks;
using FirebaseService;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace PanelsViews
{
    public class MatchResultPanelView : PanelBase, IPanelParameter<MatchResultPanelView.Data>
    {
        public class Data
        {
            public MatchData matchData;

            public Data(MatchData matchData)
            {
                this.matchData = matchData;
            }
        }

        [SerializeField] private Button[] addSetButtons;
        [SerializeField] private Button saveButton;
        [SerializeField] private Toggle walkoverToggle;

        [SerializeField] private Transform scoreInputArea;
        [SerializeField] private Transform winnerInputArea;


        [TitleGroup("HOME PLAYER")] [SerializeField]
        private TMP_Text homePlayerName;

        [SerializeField] private TMP_InputField[] firstPlayerScores;
        [SerializeField] private Button homePlayerWinnerButton;
        [SerializeField] private TMP_Text homePlayerWinnerButtonText;


        [TitleGroup("AWAY PLAYER")] [SerializeField]
        private TMP_Text awayPlayerName;

        [SerializeField] private TMP_InputField[] secondPlayerScores;
        [SerializeField] private Button awayPlayerWinnerButton;
        [SerializeField] private TMP_Text awayPlayerWinnerButtonText;


        private IFirebaseService _firebaseService;
        private int _currentAddSetButtonIndex;

        public Data Parameter { get; set; }

        [Inject]
        private void Injection(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public override async Task ShowAsync()
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
            walkoverToggle.onValueChanged.AddListener(OnToggleValueChanged);
            await DataSetInit();
            DeactivateAddSetButtons();

            addSetButtons[0].gameObject.SetActive(true);
            SetupButtonListeners();
            await base.ShowAsync();
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                scoreInputArea.gameObject.SetActive(false);
                winnerInputArea.gameObject.SetActive(true);
            }
            else
            {
                scoreInputArea.gameObject.SetActive(true);
                winnerInputArea.gameObject.SetActive(false);
            }
        }

        private async void OnSaveButtonClicked()
        {
            await _firebaseService.UpdateDataAsync
            (
                FirebaseCollectionConstants.MATCHES,
                Parameter.matchData.Id,
                newData: new Dictionary<string, object>
                {
                    { nameof(MatchData.Status), MatchStatus.COMPLETED },
                    { nameof(MatchData.Sets), GetSetsFromInput() },
                    {
                        nameof(MatchData.WinnerUser),
                        homePlayerWinnerButton.gameObject.activeSelf
                            ? Parameter.matchData.HomeUser
                            : Parameter.matchData.AwayUser
                    }
                }
            );

            // save match data 
            // update players data status
        }

        private List<MatchSetData> GetSetsFromInput()
        {
            var sets = new List<MatchSetData>();

            for (var i = 0; i < _currentAddSetButtonIndex; i++)
            {
                sets.Add(new MatchSetData
                {
                    HomeUserGames = int.Parse(firstPlayerScores[i].text),
                    AwayUserGames = int.Parse(secondPlayerScores[i].text)
                });
            }

            return sets;
        }

        private async UniTask DataSetInit()
        {
            var homeUserData = await _firebaseService.GetDataByIdAsync<UserData>(FirebaseCollectionConstants.USERS,
                Parameter.matchData.HomeUser);
            var awayUserData = await _firebaseService.GetDataByIdAsync<UserData>(FirebaseCollectionConstants.USERS,
                Parameter.matchData.AwayUser);

            homePlayerName.text = homeUserData.Data.Name;
            awayPlayerName.text = awayUserData.Data.Name;

            homePlayerWinnerButtonText.text = homeUserData.Data.Name;
            awayPlayerWinnerButtonText.text = awayUserData.Data.Name;
        }

        private void DeactivateAddSetButtons()
        {
            foreach (var button in addSetButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        private void SetupButtonListeners()
        {
            for (int i = 0; i < addSetButtons.Length; i++)
            {
                var button = addSetButtons[i];

                button.onClick.RemoveAllListeners();

                int setIndex = i;
                button.onClick.AddListener(() => HandleSetButtonClick(setIndex));
            }
        }


        private void AddNewSet()
        {
            firstPlayerScores[_currentAddSetButtonIndex].gameObject.SetActive(true);
            secondPlayerScores[_currentAddSetButtonIndex].gameObject.SetActive(true);

            _currentAddSetButtonIndex++;

            UpdateButtonStates();
        }

        private void HandleSetButtonClick(int clickedIndex)
        {
            if (clickedIndex == _currentAddSetButtonIndex)
            {
                AddNewSet();
            }
            else if (clickedIndex == _currentAddSetButtonIndex - 1 && _currentAddSetButtonIndex > 0)
            {
                RemoveSet(clickedIndex);
            }
            // If clickedIndex is not the last set, do nothing
        }

        private void RemoveSet(int setIndex)
        {
            if (_currentAddSetButtonIndex <= 0)
                return;

            // Only remove the last set
            int lastSetIndex = _currentAddSetButtonIndex - 1;

            // Deactivate and clear only the last set's scores
            firstPlayerScores[lastSetIndex].gameObject.SetActive(false);
            firstPlayerScores[lastSetIndex].text = "";
            secondPlayerScores[lastSetIndex].gameObject.SetActive(false);
            secondPlayerScores[lastSetIndex].text = "";

            _currentAddSetButtonIndex--;
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            for (int i = 0; i < addSetButtons.Length; i++)
            {
                if (i < _currentAddSetButtonIndex)
                {
                    // Show as remove buttons
                    addSetButtons[i].gameObject.SetActive(true);
                    addSetButtons[i].interactable = true;
                }
                else if (i == _currentAddSetButtonIndex)
                {
                    // Show as add button
                    addSetButtons[i].gameObject.SetActive(true);
                    addSetButtons[i].interactable = true;
                    // You might want to change button text/icon here to indicate "add"
                }
                else
                {
                    // Hide unused buttons
                    addSetButtons[i].gameObject.SetActive(false);
                    addSetButtons[i].interactable = false;
                }
            }
        }
    }
}