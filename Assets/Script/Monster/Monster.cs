using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public MonsterData monsterData;
    private int hp;
    public int Hp { get => hp; set => hp = value; }


    private Animator animator;

 

    void Start()
    {
        var meshFilter = GetComponent<SkinnedMeshRenderer>();
        animator = GetComponentInParent<Animator>();
        meshFilter.sharedMesh = monsterData.mesh;
        Hp = monsterData.hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(Hp <= 0)
        {
            animator.SetTrigger("Die");
        }
    }


}
