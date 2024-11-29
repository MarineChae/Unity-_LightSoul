using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private SphereCollider attackCollider;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private ProjectileObject projectile;
    private void Awake()
    {
        attackCollider = GetComponent<SphereCollider>();
        attackCollider.enabled = false;
    }
    internal void AllowAttack(float damage)
    {
        attackDamage = damage;
        attackCollider.enabled = true;
    }

    internal void StopAttack()
    {
        attackCollider.enabled = false;
    }
    internal void AllowSkillAttack(Vector3 positon,Vector3 destination)
    {
        var obj = Instantiate<ProjectileObject>(projectile);

        obj.transform.position = positon;

        var dir = (destination - positon).normalized;
        dir.y= 0f;
        obj.Direction = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var player = other.GetComponentInChildren<PlayerCharacter>();
            player.HP -= attackDamage;
        }
    }


}
