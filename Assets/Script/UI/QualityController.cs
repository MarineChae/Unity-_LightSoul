using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class QualityController : MonoBehaviour
{

    private TMP_Dropdown dropdown;
    private string[] qualitys = new string[] {"Low","VeryLow","Medium","High","VeryHigh","Ultra"};

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        Init();
    }


    /////////////////////////////// Private Method///////////////////////////////////
    //게임 시작 시 그래픽 세팅 초기화
    private void Init()
    {
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
    private void OnDropdownEvent(int index)
    {
        SetQuality(index);
    }
    private void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
