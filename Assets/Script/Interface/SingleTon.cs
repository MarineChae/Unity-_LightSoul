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

    //OnDestroy �� �ٸ� ��ũ��Ʈ�� OnDisable�� ȣ�� ������ ������� �ʱ⶧����
    //OnDisable�� ȣ�⿡�� null�� �����ϴ� ��찡 ���� �� �ִ�.
    //���α׷��� ����Ǵ°�쿡 null�� �����ϵ��� ���ְ�
    //OnDisable���� �̱��Ͽ� ������ �� �ִ°�쿡 nullüũ�� ���ֵ�������
    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
    private void OnDestroy()
    {
        _instance = null;
    }
}




