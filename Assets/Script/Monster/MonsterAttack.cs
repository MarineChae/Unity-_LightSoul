using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private Monster monster;
    private PlayerCharacter targetCharacter;
    private Collider attackCollider;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private ProjectileObject projectile;
    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
        attackCollider = GetComponent <Collider>();
        attackCollider.enabled = false;
    }
    internal void AllowAttack(float damage)
    {
        attackDamage = damage;
        attackCollider.enabled = true;
    }

    public void StopAttack()
    {
        if (!monster.IsParried && targetCharacter != null)
        {
            
            targetCharacter.HP -= attackDamage;

        }
        monster.IsAttack = false;
        monster.IsParried = false;
        attackCollider.enabled = false;
        targetCharacter = null;
    }
    internal void AllowSkillAttack(Vector3 positon,Vector3 destination,float damage)
    {
        attackDamage = damage;
        if (projectile == null) return;
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
