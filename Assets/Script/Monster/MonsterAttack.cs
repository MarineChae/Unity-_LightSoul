using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private Monster monster;
    private PlayerCharacter targetCharacter;
    private SphereCollider attackCollider;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private ProjectileObject projectile;
    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
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
        if (!monster.IsParried && targetCharacter != null)
        {
            
            targetCharacter.HP -= attackDamage;
            targetCharacter = null;
        }
        monster.IsParried = false;
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
            targetCharacter = other.GetComponentInChildren<PlayerCharacter>();
        }
    }


}
