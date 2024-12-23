using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{

    private GameObject poolObject;
    public Queue<IPoolingable> pool = new Queue<IPoolingable>();

    public void InitializePool(int poolCount,string path)
    {
        poolObject = Resources.Load(path) as GameObject;

        for (int i = 0; i < poolCount; i++)
        {
            pool.Enqueue(CreatePoolingObject());

        }
    }

    private IPoolingable CreatePoolingObject()
    {
        var newObj = Instantiate(poolObject);
        newObj.SetActive(false);
        newObj.transform.SetParent(transform);

        return newObj.GetComponent<IPoolingable>();
    }

}
