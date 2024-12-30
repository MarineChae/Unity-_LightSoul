using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenModeController : MonoBehaviour
{
    private TextMeshProUGUI text;
    private TMP_Dropdown dropdown;
    private string[] screenMode = new string[] { "FullScreen", "Window"};
    private void Awake()
    {

        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();
        foreach (var str in screenMode)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }
        dropdown.AddOptions(optionList);
        dropdown.value = 0;

        dropdown.onValueChanged.AddListener(OnDropdownEvent);
    }

    public void OnDropdownEvent(int index)
    {
        if(index == 0)
            Screen.SetResolution(Screen.width, Screen.height, true);
        else
            Screen.SetResolution(Screen.width, Screen.height, false);
    }


}
