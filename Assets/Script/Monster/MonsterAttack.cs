using System.Collections;
using UnityEngine;


public class MonsterAttack : MonoBehaviour
{
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private ProjectileObject projectile;
    [SerializeField]
    private GameObject hitEffectPrefab;
    private Monster monster;
    private PlayerCharacter targetCharacter;
    private Collider attackCollider;


    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetCharacter = other.GetComponentInChildren<PlayerCharacter>();
            ValidateAttack();
        }
    }

    /////////////////////////////// Public Method///////////////////////////////////
    public void AllowAttack(float damage)
    {
        attackDamage = damage;
        attackCollider.enabled = true;
    }
    public void StopAttack()
    {
        monster.IsStunned = false;
        attackCollider.enabled = false;
        targetCharacter = null;
    }
    //플레이어가 패링을 성공한경우 데미지 처리
    public void ValidateAttack()
    {
        if (!monster.IsStunned && targetCharacter != null && !targetCharacter.IsDead)
        {
            targetCharacter.TakeDamage(attackDamage);
            Vector3 dir = targetCharacter.transform.position + Vector3.up;
            var eff = Instantiate(hitEffectPrefab, dir, Quaternion.identity);
            StartCoroutine("EffectCoroutine", eff);
            SoundManager.Instance.PlaySFXSound("Sound/BowWater1");
        }
    }
    public void AllowSkillAttack(Vector3 positon, Vector3 destination, float damage, SKILL_TYPE type)
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

    /////////////////////////////// Coroutine //////////////////////////
    
    private IEnumerator EffectCoroutine(GameObject effet)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(effet);
    }
}
