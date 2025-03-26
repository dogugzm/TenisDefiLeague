using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeagueItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI number;
    [SerializeField] Image image;
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
        name.text = data.Name;
        number.text = data.Users.Count.ToString();
    }
}