using UnityEngine;
using System.Collections.Generic;

public class GameManager : SingleTon<GameManager>, IUpdatable
{
    public ObjectPool objectPool;
    public List<InvectoryGrid> grids = new List<InvectoryGrid>();
    private int currentWidth = 0;
    private int currentHeight = 0;
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.UnSubscribe(this, true, false, false);
    }

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
        pool.AddComponent<ObjectPool>().InitializePool(10, "prefabs/WayPoint");
        objectPool = pool.GetComponent<ObjectPool>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void FixedUpdateWork() { }
    public void UpdateWork()
    {

    }
    public void LateUpdateWork()
    {

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

    public void AddGird(InvectoryGrid grid)
    {
        grids.Add(grid);

    }

}

