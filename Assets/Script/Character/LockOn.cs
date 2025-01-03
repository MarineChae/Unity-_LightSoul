using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private float viewAngle = 180.0f;
    private Transform target;
    private HashSet<Transform> targets = new HashSet<Transform>();
    private float viewRadius = 10.0f;
    private PlayerCharacter character;

    private void Awake()
    {
        character = GetComponentInParent<PlayerCharacter>();
    }
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Vector3 dirToTarget = (other.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                targets.Add(other.transform);
                UpdateNearPlayer();
            }
            else
            {
                if (targets.Contains(other.transform))
                {
                     if (other.transform == target)
                         target = null;
                     targets.Remove(other.transform);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (other.transform == target)
            {
                character.IsLockOn=false;
                target = null;
            }
               
            targets.Remove(other.transform);
        }

    }
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

}
