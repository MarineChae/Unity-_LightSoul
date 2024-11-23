using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public CapsuleCollider capsuleCollider;
    public float attackRate = 1.5f;
    private void Start()
    {
        tag = "Weapon";
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        capsuleCollider.height = 1.5f;
        capsuleCollider.radius = 0.3f;
        capsuleCollider.center = new Vector3(0, 0.8f, 0);
        capsuleCollider.isTrigger = true;
        capsuleCollider.providesContacts = true;
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
