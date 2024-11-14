using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    CapsuleCollider capsuleCollider;
    public float attackRate = 1.5f;
    private void Start()
    {
        tag = "Weapon";
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        capsuleCollider.height = 2.5f;
        capsuleCollider.radius = 0.3f;
        capsuleCollider.center = new Vector3(0, 1.0f, 0);
        capsuleCollider.isTrigger = true;
        capsuleCollider.providesContacts = true;
    }


    public void Attack()
    {
        StopCoroutine("Swip");
        StartCoroutine("Swip");
    }

    IEnumerator Swip()
    {
        yield return new WaitForSeconds(0.1f);
        capsuleCollider.enabled = true;
        Debug.Log("attackstart");
        yield return new WaitForSeconds(0.4f);
        capsuleCollider.enabled = false;
        Debug.Log("attackend");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            var monster = other.GetComponentInChildren<Monster>();
            monster.Hp -= 50;
            Debug.Log("OnTriggerEnter " + other.gameObject.name);
        }
    }

}
