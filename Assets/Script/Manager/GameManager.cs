using UnityEngine;


/// <summary>
/// 오브젝트 풀링을 위한 매니저 
/// </summary>
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

