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

                //���Ͱ� �ڵ��ƺ��� �ִ� ��� �÷��̾� ������ ȸ����Ű��
                if (monster.MonsterRangeChecker.Target == null)
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
                monster.IsStunned = true;
                monster.Animator.SetTrigger("Stunned");
                playerAttack.TargetMonster = monster;
            }
        }
    }

    //�÷��̾� ���� �ʱ�ȭ, ����� �����ΰ�� �ٸ��� �ʱ�ȭ����
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
        //�ǰ�����Ʈ
        Vector3 dir = (other.transform.position + transform.position) * 0.5f;
        var eff = Instantiate(HitPrefab, dir, Quaternion.identity);
        //����Ʈ�� ������ destory�ϴ� �ڷ�ƾ
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
