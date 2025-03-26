using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LeaguePlayerItem : MonoBehaviour
{
    private string _id;

    private UserManager _userManager;
    private MatchService _matchService;

    [Inject]
    private void Injection(UserManager userManager, MatchService matchService)
    {
        _userManager = userManager;
        _matchService = matchService;
    }


    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Status;
    [SerializeField] List<Image> LastMatches;
    [SerializeField] Button offer;

    private void OnEnable()
    {
        offer.onClick.AddListener(OfferButtonClicked);
    }

    private async void OfferButtonClicked()
    {
        var result = await _matchService.TryOfferMatch(_id);
        if (!result) return;
        offer.gameObject.SetActive(false);
        Status.gameObject.SetActive(true);
        Status.text = UserStatus.IN_MATCH.ToString();
    }

    private void OnDisable()
    {
        offer.onClick.RemoveListener(OfferButtonClicked);
    }

    public void SetData(UserData userData)
    {
        _id = userData.UserID;
        Name.text = userData.Name;
        Status.text = ((UserStatus)userData.UserStatus).ToString();
        bool isReady = (UserStatus)userData.UserStatus == UserStatus.AVAILABLE &&
                       _userManager.Data.UserID != userData.UserID;
        offer.gameObject.SetActive(isReady);
        Status.gameObject.SetActive(!isReady);
    }

    private void SetMatches(List<bool> matchList)
    {
        for (int i = 0; i < matchList.Count; i++)
        {
            LastMatches[i].color = matchList[i] ? Color.green : Color.red;
        }
    }
}