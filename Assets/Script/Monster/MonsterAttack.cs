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
    [SerializeField]
    private GameObject hitEffectPrefab;
    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
    }
    internal void AllowAttack(float damage)
    {
        attackDamage = damage;
        attackCollider.enabled = true;
    }

    public void ValidateAttack()
    {
        if (!monster.IsParried && targetCharacter != null)
        {
            targetCharacter.TakeDamage(attackDamage);
            Vector3 dir = targetCharacter.transform.position + Vector3.up;
            var eff = Instantiate(hitEffectPrefab, dir, Quaternion.identity);
            StartCoroutine("EffectCoroutine", eff);
        }
    }
    public void StopAttack()
    {
        monster.IsParried = false;
        attackCollider.enabled = false;
        targetCharacter = null;
    }
    internal void AllowSkillAttack(Vector3 positon, Vector3 destination, float damage, SKILL_TYPE type)
    {
        attackDamage = damage;
        if (SKILL_TYPE.RUSH == type || SKILL_TYPE.NONE == type)
        {
            attackCollider.enabled = true;
        }
        else if(SKILL_TYPE.PROJECTILE == type)
        {
            var obj = Instantiate<ProjectileObject>(projectile);
            obj.transform.position = positon;
            var dir = (destination - positon).normalized;
            dir.y = 0f;
            obj.Direction = dir;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            targetCharacter = other.GetComponentInChildren<PlayerCharacter>();
            ValidateAttack();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {


        }
    }
    private IEnumerator EffectCoroutine(GameObject effet)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(effet);
    }
}
