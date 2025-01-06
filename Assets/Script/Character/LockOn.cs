using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    private readonly float viewAngle = 180;
    private Monster target;
    private HashSet<Transform> targets = new HashSet<Transform>();
    private readonly float viewRadius = 10.0f;
    private PlayerCharacter character;

    private void Awake()
    {
        character = GetComponentInParent<PlayerCharacter>();
    }
    public Monster Target
    {
        get { return target; }
        set { target = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            targets.Add(other.transform);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Vector3 dirToTarget = (other.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                UpdateNearPlayer();
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (other.transform.Equals(target.transform))
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

        foreach (Transform targetMonster in targets)
        {//fake null È®ÀÎ

            float dist = (targetMonster.position - transform.position).sqrMagnitude;

            if (dist < closestDist)
            {
                closestDist = dist;
                target = targetMonster.GetComponent<Monster>();
            }
        }
    }
    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
    }
}
