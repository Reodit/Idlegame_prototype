using System;

namespace GameData
{
    [Serializable]
    public class MonsterData : IIdentifiable
    {
        public int id;
        public string prefabName;
        public string monsterName;
        public int maxHp;
        public int rewardGold;

        public int GetID()
        {
            return id;
        }

        public string GetDataName()
        {
            return monsterName;
        }
    }
}


