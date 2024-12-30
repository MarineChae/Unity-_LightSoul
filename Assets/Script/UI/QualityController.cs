using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class QualityController : MonoBehaviour
{

    private TextMeshProUGUI text;
    private TMP_Dropdown dropdown;
    private string[] qualitys = new string[] {"Low","VeryLow","Medium","High","VeryHigh","Ultra"};

    
    private void Awake()
    {

        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();
        foreach (var str in qualitys)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }
        dropdown.AddOptions(optionList);
        dropdown.value = 6;
        QualitySettings.SetQualityLevel(6);
        dropdown.onValueChanged.AddListener(OnDropdownEvent);
    }

    public void OnDropdownEvent(int index)
    {
        SetQuality(index);
    }

    private void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
