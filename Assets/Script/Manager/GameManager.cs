using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : SingleTon<GameManager>, IUpdatable
{

    void Start()
    {
        Init();
    }

    void Init()
    {
        GameObject update = new GameObject("UpdateManager");
        update.transform.SetParent(transform);
        update.AddComponent<UpdateManager>();

    }

    public void TestFunc()
    {
        Debug.Log("test!");
    }

public void FixedUpdateWork() { }
    public void UpdateWork() { }
    public void LateUpdateWork() { }

}
