using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{

    [SerializeField]
    private ResolutionController resolutionController;
    [SerializeField]
    private QualityController qualityController;
    [SerializeField]
    private Slider sliderBGMSound;
    [SerializeField]
    private Slider sliderSFXSound;
    [SerializeField]
    private Button exitButton;
    public ResolutionController ResolutionController { get => resolutionController; }
    public QualityController QualityController { get => qualityController;  }

    /////////////////////////////// Life Cycle ///////////////////////////////////

    private void Awake()
    {
        exitButton.onClick.AddListener(ExitButton);
    }
    private void Start()
    {
        sliderBGMSound.value = SoundManager.Instance.MasterVolumeBGM;
        sliderSFXSound.value = SoundManager.Instance.MasterVolumeSFX;
    }
    private void Update()
    {
        if (sliderBGMSound != null)
        {
            SoundManager.Instance.MasterVolumeBGM = sliderBGMSound.value;
            SoundManager.Instance.SetBGMVolume(sliderBGMSound.value);
        }
        if (sliderSFXSound != null)
        {
            SoundManager.Instance.MasterVolumeSFX = sliderSFXSound.value;
        }
    }

    /////////////////////////////// Private Method///////////////////////////////////
    private void ExitButton()
    {
        UIManager.Instance.RemoveCanvas();
    }

}
