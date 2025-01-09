using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneContoller : MonoBehaviour
{
    [SerializeField]
    Image prograssBar;
    static string nextScene;


    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }
    /////////////////////////////// Public Method///////////////////////////////////
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    /////////////////////////////// Coroutine //////////////////////////
    
    //씬을 비동기로 전환하기 위한 코루틴
    IEnumerator LoadSceneProcess()
    {
        AsyncOperation aOp =  SceneManager.LoadSceneAsync(nextScene);
        aOp.allowSceneActivation = false;

        float timer = 0f;
        while (!aOp.isDone)
        {
            yield return null;

            if(aOp.progress < 0.9f)
            {
                prograssBar.fillAmount = aOp.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                prograssBar.fillAmount = Mathf.Lerp(0.9f,1.0f, timer);
                if(prograssBar.fillAmount >= 1.0f)
                {
                    aOp.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }


}
