using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public CapsuleCollider capsuleCollider;
    private PlayerAttack playerAttack;
    private ItemData itemData;
    public float attackRate = 1.5f;
    public GameObject hitPrefab;
    private PlayerCharacter playerCharacter;

    public ItemData ItemData { get => itemData; set => itemData = value; }

    private void Awake()
    {

        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        playerCharacter = GetComponentInParent<PlayerCharacter>();
        playerAttack = GetComponentInParent<PlayerAttack>();
    }
    //플레이어 무기 초기화, 무기와 방패인경우 다르게 초기화해줌
    public void InitCollider()
    {
        capsuleCollider.enabled = false;
        capsuleCollider.isTrigger = true;
        capsuleCollider.providesContacts = true;
        if (CompareTag("Weapon"))
        {
            capsuleCollider.height = 1.5f;
            capsuleCollider.radius = 0.3f;
            capsuleCollider.center = new Vector3(0, 0.8f, 0);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Weapon"))
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                var monster = other.GetComponentInChildren<Monster>();
                if (monster.IsStunned)
                {
                    monster.TakeDamage(itemData.damage * 3);
                }
                else
                {
                    monster.TakeDamage(itemData.damage);
                }
                HitEffect(other);

                //몬스터가 뒤돌아보고 있는 경우 플레이어 쪽으로 회전시키게
                if (monster.monsterRangeChecker.Target == null)
                    monster.RotateToTarget(transform, true);
            }
        }
        else if (this.CompareTag("Shield"))
        {
            //플레이어의 방패와 몬스터의 공격이 맞닿으면 패링처리
            if (other.gameObject.CompareTag("MonsterWeapon") && !playerCharacter.IsHit)
            {
                HitEffect(other);
                ///sound
                SoundManager.Instance.PlaySFXSound("Sound/HammerImpact12");

                var monster = other.GetComponentInParent<Monster>();
                monster.IsParried = true;
                monster.Animator.SetTrigger("Stunned");
                playerAttack.TargetMonster = monster;
            }
        }
    }

    private void HitEffect(Collider other)
    {
        //피격이펙트
        Vector3 dir = (other.transform.position + transform.position) * 0.5f;
        var eff = Instantiate(hitPrefab, dir, Quaternion.identity);
        //이펙트가 끝나면 destory하는 코루틴
        StartCoroutine("EffectCoroutine", eff);
    }

    private IEnumerator EffectCoroutine(GameObject effet)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(effet);
    }
}
