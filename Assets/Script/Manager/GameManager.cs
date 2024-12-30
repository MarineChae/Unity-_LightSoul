using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : SingleTon<GameManager>
{
    private ObjectPool objectPool;

    protected override void Init()
    {

        GameObject pool = new GameObject("ObjectPool");
        pool.transform.SetParent(transform);
        pool.AddComponent<ObjectPool>().InitializePool(10, "prefabs/WayPoint");
        objectPool = pool.GetComponent<ObjectPool>();

    }
    public IPoolingable GetPoolingObject()
    {
        var obj = objectPool.pool.Dequeue();

        return obj;
    }
    public void ReturnPoolingObject(IPoolingable returnObject)
    {
        objectPool.pool.Enqueue(returnObject);
    }

}

