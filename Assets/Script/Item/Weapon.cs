using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject hitPrefab;
    private CapsuleCollider capsuleCollider;
    private PlayerAttack playerAttack;
    private ItemData itemData;
    private PlayerCharacter playerCharacter;

    /////////////////////////////// Life Cycle ///////////////////////////////////
    private void Awake()
    {
        CapsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        playerCharacter = GetComponentInParent<PlayerCharacter>();
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("Weapon"))
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
                if (monster.MonsterRangeChecker.Target == null)
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
                monster.IsStunned = true;
                monster.Animator.SetTrigger("Stunned");
                playerAttack.TargetMonster = monster;
            }
        }
    }

    //플레이어 무기 초기화, 무기와 방패인경우 다르게 초기화해줌
    public void InitCollider()
    {
        CapsuleCollider.enabled = false;
        CapsuleCollider.isTrigger = true;
        CapsuleCollider.providesContacts = true;
        if (CompareTag("Weapon"))
        {
            CapsuleCollider.height = 1.5f;
            CapsuleCollider.radius = 0.3f;
            CapsuleCollider.center = new Vector3(0, 0.8f, 0);
        }
        
    }

    private void HitEffect(Collider other)
    {
        //피격이펙트
        Vector3 dir = (other.transform.position + transform.position) * 0.5f;
        var eff = Instantiate(HitPrefab, dir, Quaternion.identity);
        //이펙트가 끝나면 destory하는 코루틴
        StartCoroutine("EffectCoroutine", eff);
    }

    /////////////////////////////// Coroutine //////////////////////////
    private IEnumerator EffectCoroutine(GameObject effet)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(effet);
    }

    /////////////////////////////// Property ///////////////////////////////////
    public ItemData ItemData { get => itemData; set => itemData = value; }
    public GameObject HitPrefab { get => hitPrefab; set => hitPrefab = value; }
    public CapsuleCollider CapsuleCollider { get => capsuleCollider; set => capsuleCollider = value; }
}
