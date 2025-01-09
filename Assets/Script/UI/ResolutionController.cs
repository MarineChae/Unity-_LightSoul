using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Toggle;

public class ResolutionController : MonoBehaviour
{
    [SerializeField]
    private Toggle screenMode;
    private TMP_Dropdown dropdown;
    private List<Resolution> resolutions = new List<Resolution>();
    private int optimalResolutionIndex = 0;
    private bool currentScreenMode = true;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        InitResolutions();
    }
    /////////////////////////////// Private Method///////////////////////////////////
    private void InitResolutions()
    {
        dropdown.ClearOptions();
        resolutions.Add(new Resolution { width = 1280, height = 720 });
        resolutions.Add(new Resolution { width = 1280, height = 800 });
        resolutions.Add(new Resolution { width = 1440, height = 900 });
        resolutions.Add(new Resolution { width = 1600, height = 900 });
        resolutions.Add(new Resolution { width = 1680, height = 1050 });
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 1920, height = 1200 });
        resolutions.Add(new Resolution { width = 2048, height = 1280 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
        resolutions.Add(new Resolution { width = 2560, height = 1600 });
        resolutions.Add(new Resolution { width = 2880, height = 1800 });
        resolutions.Add(new Resolution { width = 3480, height = 2160 });

        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                optimalResolutionIndex = i;
                option += " *";
            }
            optionList.Add(new TMP_Dropdown.OptionData(option));
        }

        dropdown.AddOptions(optionList);
        dropdown.value = optimalResolutionIndex;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnDropdownEvent);
        Resolution resolution = resolutions[optimalResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        screenMode.isOn = true;
        screenMode.onValueChanged.AddListener(onToggleEvent);
    }

    private void OnDropdownEvent(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, currentScreenMode);
    }

    private void onToggleEvent(bool trigger)
    {
        currentScreenMode = trigger;
        Screen.SetResolution(Screen.width, Screen.height, trigger);

    }


}
