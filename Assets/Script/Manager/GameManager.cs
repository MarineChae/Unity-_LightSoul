using UnityEngine;


public class GameManager : SingleTon<GameManager>, IUpdatable
{
    public ObjectPool objectPool;

    void Start()
    {
        Init();
    }

    void Init()
    {
        GameObject update = new GameObject("UpdateManager");
        update.transform.SetParent(transform);
        update.AddComponent<UpdateManager>();

        GameObject pool = new GameObject("ObjectPool");
        pool.transform.SetParent(transform);
        pool.AddComponent<ObjectPool>().InitializePool(10,"prefabs/WayPoint");
        objectPool = pool.GetComponent<ObjectPool>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TestFunc()
    {
        Debug.Log("test!");
    }

    public void FixedUpdateWork() { }
    public void UpdateWork() { }
    public void LateUpdateWork() { }


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
