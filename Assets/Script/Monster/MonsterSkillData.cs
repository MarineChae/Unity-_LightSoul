using System;

public enum SKILL_TYPE
{
    RUSH,
    PROJECTILE,
    NONE
}

[Serializable]
public class MonsterSkillData
{
    public int id = 0;
    public string name = "";
    public float skillDamage;
    public float coolDown;
    public float remainCoolDown;
    public SKILL_TYPE skillType;

}
[Serializable]
public class MonsterSkillDataArray
{
    public MonsterSkillData[] MonsterSkillDatas;
}
