using System;

[Serializable]
public class Skill
{
    public int id;
    public string skillName;
    public float cooldownDuration;
    public int skillDamage;
    public int hitCount;

    public Skill(Skill _skill)
    {
        id = _skill.id;
        skillName = _skill.skillName;
        cooldownDuration = _skill.cooldownDuration;
        skillDamage = _skill.skillDamage;
        hitCount = _skill.hitCount;
    }
}