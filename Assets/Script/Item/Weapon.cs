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
    //�÷��̾� ���� �ʱ�ȭ, ����� �����ΰ�� �ٸ��� �ʱ�ȭ����
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

                //���Ͱ� �ڵ��ƺ��� �ִ� ��� �÷��̾� ������ ȸ����Ű��
                if (monster.monsterRangeChecker.Target == null)
                    monster.RotateToTarget(transform, true);
            }
        }
        else if (this.CompareTag("Shield"))
        {
            //�÷��̾��� ���п� ������ ������ �´����� �и�ó��
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
        //�ǰ�����Ʈ
        Vector3 dir = (other.transform.position + transform.position) * 0.5f;
        var eff = Instantiate(hitPrefab, dir, Quaternion.identity);
        //����Ʈ�� ������ destory�ϴ� �ڷ�ƾ
        StartCoroutine("EffectCoroutine", eff);
    }

    private IEnumerator EffectCoroutine(GameObject effet)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(effet);
    }
}
