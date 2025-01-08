using UnityEngine;

public enum PoolingType
{
    WAYPOINT = 0,
    EFFECT = 1
}

public interface IPoolingable
{
    public PoolingType GetPoolingType();

    public void Activate(Vector3 position);

    public void DeActivate();


}
