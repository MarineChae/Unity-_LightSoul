using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterSkillData
{
    public int id = 0;
    public string name = "";
    public float skillDamage;
    public float coolDown;
    public float remainCoolDown;
}
[Serializable]
public class MonsterSkillDataArray
{
    public MonsterSkillData[] MonsterSkillDatas;
}
