using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{

    [SerializeField]
    private ResolutionController resolutionController;
    [SerializeField]
    private QualityController qualityController;

    public ResolutionController ResolutionController { get => resolutionController; }
    public QualityController QualityController { get => qualityController;  }

    private void Awake()
    {

    
    }


}
