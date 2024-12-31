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
    public ResolutionController ResolutionController { get => resolutionController; }
    public QualityController QualityController { get => qualityController;  }
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


}
