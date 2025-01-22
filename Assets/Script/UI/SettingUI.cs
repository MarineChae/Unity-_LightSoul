using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour ,IUpdatable
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
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }
    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }
    private void Awake()
    {
        exitButton.onClick.AddListener(ExitButton);
        sliderBGMSound.value = SoundManager.Instance.MasterVolumeBGM;
        sliderSFXSound.value = SoundManager.Instance.MasterVolumeSFX;
    }
    public void FixedUpdateWork() { }
    public void UpdateWork()
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
    public void LateUpdateWork() { }


    /////////////////////////////// Private Method///////////////////////////////////
    private void ExitButton()
    {
        UIManager.Instance.RemoveCanvas();
    }

}
