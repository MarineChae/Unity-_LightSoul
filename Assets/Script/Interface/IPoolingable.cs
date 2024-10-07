using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolingType
{
    WAYPOINT = 0
}

public interface IPoolingable
{
    public PoolingType GetPoolingType();

    public void Activate();

    public void DeActivate();


}
