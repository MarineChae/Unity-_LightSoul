using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool isApplicationQuitting =false;
    public static T Instance
    {
        get
        {
            if (isApplicationQuitting)
                return null;
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name + "AutoCreated";
                    _instance = obj.AddComponent<T>();
                    Debug.Log(obj.name);
                }
            }
            
            return _instance;
        }

    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject); 
        }

    }

    //OnDestroy 와 다른 스크립트의 OnDisable의 호출 순서가 보장되지 않기때문에
    //OnDisable의 호출에서 null을 참조하는 경우가 생길 수 있다.
    //프로그램이 종료되는경우에 null을 리턴하도록 해주고
    //OnDisable에서 싱글턴에 접근할 수 있는경우에 null체크를 해주도록하자
    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
    private void OnDestroy()
    {
        _instance = null;
    }
}




