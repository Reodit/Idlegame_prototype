using System;
using System.Collections.Generic;
using GameData;
using Managers;

[Serializable]
public class HeroData : IIdentifiable
{
    public int id;
    public string heroName;
    public int hp;
    public int mp;
    public List<Skill> skills;
    public string prefabName;

    public HeroData(int _id, string _heroName, int _hp, int _mp, List<Skill> _skills, string _prefabName)
    {
        id = _id;
        heroName = _heroName;
        hp = _hp;
        mp = _mp;
        skills = _skills;
        prefabName = _prefabName;
    }

    public HeroData(HeroData _heroData)
    {
        id = _heroData.id;
        heroName = _heroData.heroName;
        hp = _heroData.hp;
        mp = _heroData.mp;
        skills = new List<Skill>(_heroData.skills);
        prefabName = _heroData.prefabName;
    }
    
    public int GetID()
    {
        return id;
    }

    public string GetDataName()
    {
        return heroName;
    }
}