using Configs;
using Cysharp.Threading.Tasks;
using PanelsViews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Views;

public class LeagueView : MonoBehaviour
{
    public class Data
    {
        public LeagueData LeagueData { get; set; }
        public LogoView.Data LogoViewData { get; set; }
    }
    
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text dates;
    [SerializeField] TMP_Text number;
    [SerializeField] Image bgImage;
    
    [field: SerializeField] public Button Button { get; set; }
    
    private void OnEnable()
    {
        Button.onClick.AddListener(OnBtnClicked);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(OnBtnClicked);
    }

    private void OnBtnClicked()
    {
    }

    public void SetData(Data data)
    {
        bgImage.color = new Color(Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f));
        name.text = data.LeagueData.Name;
        number.text = data.LeagueData.Users.Count.ToString();
        dates.text = DateFormatter.FormatLeagueDateRange(data.LeagueData.StartDate, data.LeagueData.EndDate);
    }
    
    private async UniTaskVoid OnLeagueButtonClicked()
    {
        // var leagueData = await _leagueService.GetLeague(Parameter.LeagueID);
        // var userList = await _leagueService.GetAllLeaguePlayers(leagueData);
        //
        // var panelData = new LeagueProfilePanel.Data(
        //     Parameter,
        //     leagueData.Users.Count,
        //     leagueData.Users.Count,
        //     userList
        // );
        // await _panelService.ShowPanelAsync<LeagueProfilePanel, LeagueProfilePanel.Data>(panelData);
    }
}