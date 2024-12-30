using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    private Button resumeButton;
    private Button mainMenuButton;
    private Button exitButton;
    private Button settingButton;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        resumeButton = transform.Find("Resume").GetComponent<Button>();
        resumeButton.onClick.AddListener(UIManager.Instance.Pause);

        mainMenuButton = transform.Find("MainMenu").GetComponent<Button>();
        mainMenuButton.onClick.AddListener(UIManager.Instance.MainMenu);

        exitButton = transform.Find("Exit").GetComponent<Button>();
        exitButton.onClick.AddListener(UIManager.Instance.Exit);

        settingButton = transform.Find("Setting").GetComponent<Button>();
        settingButton.onClick.AddListener(UIManager.Instance.Setting);
    }

}
