using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            return _instance;
        }

    }

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        if (Application.isPlaying)
        {
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }

}




