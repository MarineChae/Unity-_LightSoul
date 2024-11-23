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
    public float moveSpeed;
    public string behaviorTreeName;
}
[Serializable]
public class MonsterDataArray
{
    public MonsterData[] MonsterDatas;
}
