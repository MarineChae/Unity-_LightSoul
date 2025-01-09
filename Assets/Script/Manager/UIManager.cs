using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIManager : SingleTon<UIManager>
{
    private List<Canvas> activateCanvases = new List<Canvas>();
    private InventoryController inventoryController;
    private PotionSlot potionSlot;
    private PauseUI pauseUI;
    private SettingUI settingUI;

    /////////////////////////////// Life Cycle ///////////////////////////////////
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
        settingUI = FindObjectOfType<SettingUI>();
        if(settingUI != null)
            settingUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && pauseUI != null)
        {
            if(activateCanvases.Count <= 0)
            {
                Pause();
            }
            else
            {
                RemoveCanvas();
                if (activateCanvases.Count <= 0)
                    Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }


    /////////////////////////////// Public Method///////////////////////////////////
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
    public void Setting()
    {
        settingUI.gameObject.SetActive(true);
        var canvas = settingUI.GetComponent<Canvas>();
        AddCanvas(canvas);
    }
    public void AddCanvas(Canvas canvas)
    {
        activateCanvases.Add(canvas);
    }

    public void RemoveCanvas()
    {
        activateCanvases[0].gameObject.SetActive(false);
        activateCanvases.Remove(activateCanvases[0]);

    }
    /////////////////////////////// Property /////////////////////////////////

    public InventoryController InventoryController { get => inventoryController; }
    public PotionSlot PotionSlot { get => potionSlot; }

}
