using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LeagueItem : MonoBehaviour
{
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

    public void SetData(LeagueData data)
    {
        bgImage.color = new Color(Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f));
        name.text = data.Name;
        number.text = data.Users.Count.ToString();

        if (data.StartDate != default && data.EndDate != default)
        {
            string startMonth = data.StartDate.ToString("MMMM");
            string endMonth = data.EndDate.ToString("MMMM");
            string year = data.EndDate.Year.ToString();
            dates.text = $"{startMonth} - {endMonth} {year}";
        }
        else
        {
            dates.text = "TBD";
        }
    }
}