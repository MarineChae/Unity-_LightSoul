using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    CapsuleCollider capsuleCollider;
    public float attackRate = 1.5f;
    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
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
        yield return new WaitForSeconds(0.3f);
        capsuleCollider.enabled = false;
        Debug.Log("attackend");

    }


}
