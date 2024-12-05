using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public CapsuleCollider capsuleCollider;
    public float attackRate = 1.5f;
    public GameObject hitPrefab;
    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

    }

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
                monster.Hp -= 50;

                Vector3 dir = (other.transform.position + transform.position) * 0.5f;

                var eff = Instantiate(hitPrefab, dir, Quaternion.identity);
                StartCoroutine("EffectCoroutine", eff);
                Debug.Log("OnTriggerEnter " + other.gameObject.name);
            }
        }
        else if (this.CompareTag("Shield"))
        {
            if (other.gameObject.CompareTag("MonsterWeapon"))
            {
                //디버그용./////
                Vector3 dir = (other.transform.position + transform.position) * 0.5f;
                var eff = Instantiate(hitPrefab, dir, Quaternion.identity);
                StartCoroutine("EffectCoroutine", eff);
                Debug.Log("parring " + other.gameObject.name);
                ////////
                var monster = other.GetComponentInParent<Monster>();
                monster.IsParried = true;
                monster.Animator.SetTrigger("Stunned");
            }
        }
    }

    private IEnumerator EffectCoroutine(GameObject effet)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(effet);
    }
}
