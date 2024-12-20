using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterData
{
    public int id = 0;
    public string name = "";
    public int hp;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public float meleeDamage;
    public float skillDamage;
    public string behaviorTreeName;
}
[Serializable]
public class MonsterDataArray
{
    public MonsterData[] MonsterDatas;
}
