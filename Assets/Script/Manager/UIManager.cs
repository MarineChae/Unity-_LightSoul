using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingleTon<UIManager>
{
    private InventoryController inventoryController;
    private PotionSlot potionSlot;
    private PauseUI pauseUI;
    public InventoryController InventoryController { get => inventoryController; }
    public PotionSlot PotionSlot { get => potionSlot;}

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
        inventoryController = FindObjectOfType<InventoryController>();
        potionSlot = FindObjectOfType<PotionSlot>();
        pauseUI = FindObjectOfType<PauseUI>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && pauseUI != null)
        {
            Pause();
        }
    }
    public void Pause()
    {
        Time.timeScale = Time.timeScale <= 0 ? 1 : 0;
        bool isPause = Time.timeScale <= 0;
        pauseUI.gameObject.SetActive(isPause);
        Cursor.visible = isPause;
        if (isPause)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
    public void MainMenu()
    {
        Pause();
        Cursor.lockState = CursorLockMode.None;
        LoadingSceneContoller.LoadScene("StartScene");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
