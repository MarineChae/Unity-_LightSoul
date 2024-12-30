
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{

    public void NewGame()
    {
        LoadingSceneContoller.LoadScene("MainScene");
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ContinueGame()
    {
        LoadingSceneContoller.LoadScene("TestScene");
    }
    public void Setting()
    {


    }
   public void Exit()
    {
        Application.Quit();
    }
}
