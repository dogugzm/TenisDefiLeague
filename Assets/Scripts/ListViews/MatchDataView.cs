using System;
using Cysharp.Threading.Tasks.Triggers;
using FirebaseService;
using PanelService;
using PanelsViews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Views;

namespace ListViews
{
    public class MatchDataView : MonoBehaviour
    {
        [SerializeField] private TMP_Text firstPlayerName;
        [SerializeField] private Image firstPlayerImage;

        [SerializeField] private TMP_Text secondPlayerName;
        [SerializeField] private Image secondPlayerImage;

        [SerializeField] private TMP_Text dateText;
        [SerializeField] private Button setButton;

        private IFirebaseService _firebaseService;
        private IPanelService _panelService;

        [Inject]
        private void Injection(IFirebaseService firebaseService, IPanelService panelService)
        {
            _firebaseService = firebaseService;
            _panelService = panelService;
        }

        private void OnDisable()
        {
            setButton.onClick.RemoveAllListeners();
        }

        public async void Init(MatchData data)
        {
            var homeUser = await _firebaseService.GetDataByIdAsync<UserData>("Users", data.HomeUser);
            var awayUser = await _firebaseService.GetDataByIdAsync<UserData>("Users", data.AwayUser);

            firstPlayerName.text = homeUser.Data.Name;
            secondPlayerName.text = awayUser.Data.Name;

            dateText.text = $"{data.StartDate}--{data.EndDate}";


            // setButton.onClick.AddListener(() =>
            // {
            //     _panelService.ShowPanelAsync<MatchResultPanelView, MatchResultPanelView.Data>(
            //         new MatchResultPanelView.Data(data));
            // });
        }
    }
}