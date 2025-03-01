using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangeChecker : MonoBehaviour
{
    [SerializeField]
    private float viewRadius;
    [SerializeField]
    [Range(0, 360)]
    private float viewAngle;
    private Transform target;
    private HashSet<Transform> targets = new HashSet<Transform>();
    private readonly float updateTime = 1.0f;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    void Start()
    {
        InvokeRepeating(nameof(UpdateNearPlayer), 0, updateTime);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 dirToTarget = (other.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                targets.Add(other.transform);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targets.Remove(other.transform);
        }
    }
    /////////////////////////////// Private Method///////////////////////////////////
    private void UpdateNearPlayer()
    {
        float closestDist = viewRadius * viewRadius;
        target = null;

        foreach (Transform targetplayer in targets)
        {
            float dist = (targetplayer.position - transform.position).sqrMagnitude;

            if (dist < closestDist)
            {
                closestDist = dist;
                target = targetplayer;
            }
        }
    }
    /////////////////////////////// Public Method///////////////////////////////////
    public Vector3 DirectionFromAngle(float angleDegree, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegree += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegree + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegree + 90) * Mathf.Deg2Rad));
    }
    /////////////////////////////// Property /////////////////////////////////
    public Transform Target { get => target; set => target = value; }
    public HashSet<Transform> Targets { get => targets; set => targets = value; }
    public float ViewRadius { get => viewRadius; set => viewRadius = value; }
    public float ViewAngle { get => viewAngle; set => viewAngle = value; }
}
