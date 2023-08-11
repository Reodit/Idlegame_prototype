using System;

namespace GameData
{
    [Serializable]
    public class StageData : IIdentifiable
    {
        public int id;
        public string stageName;
        public SpawnData[] monsterSequence;
        public int GetID()
        {
            return id;
        }

        public string GetDataName()
        {
            return stageName;
        }
    }
    
    [Serializable]
    public class SpawnData
    {
        public int monsterID;
        public int monsterSpacing;
    }
}
