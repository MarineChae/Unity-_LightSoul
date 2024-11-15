using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    public MonsterData monsterData;
    private int hp;
    [SerializeField]
    private float viewRadius;
    [SerializeField]
    [Range(0,360)]
    private float viewAngle;
    [SerializeField]
    private float walkSpeed;

    private NavMeshAgent navMeshAgent;
    private Transform target;
    private HashSet<Transform> targets = new HashSet<Transform>();
    private Animator animator;
    private float updateTime = 1.0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        var meshFilter = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        meshFilter.sharedMesh = monsterData.mesh;
        Hp = monsterData.hp;
        InvokeRepeating(nameof(UpdateNearPlayer), 0, updateTime);
    }

    private void FixedUpdate()
    {
        if(target != null && Hp > 0)
        navMeshAgent.SetDestination(target.position);
    }
    void Update()
    {
        if (Hp <= 0)
        {
            animator.SetBool("Die",true);
        }
        else
        {
            if (navMeshAgent.velocity != Vector3.zero)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }
    }

    public Vector3 DirectionFromAngle(float angleDegree, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegree += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegree + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegree + 90) * Mathf.Deg2Rad));
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


    public int Hp { get => hp; set => hp = value; }
    public float ViewRadius { get => viewRadius; set => viewRadius = value; }
    public float ViewAngle { get => viewAngle; set => viewAngle = value; }
}
