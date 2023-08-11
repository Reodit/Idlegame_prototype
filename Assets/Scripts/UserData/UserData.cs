using System;
using System.Collections.Generic;
using GameData;

[Serializable]
public struct UserData : IIdentifiable
{
    // 스킬 사용 이력
    // Key : 캐릭터 ID,
    // Key : 스킬 ID, 스킬 사용 시간 
    public Dictionary<int, Dictionary<int, DateTime>> LastSkillUsedTimes;
        
    // TODO 임시 보유 캐릭터 목록 데이터 
    // 보유 캐릭터 목록 (* 이후에 저장된 캐릭터 능력치 적용)
    public List<int> userHeroes;

    public int playerMoney;
    public int playerDiamond;
    public int playerTicket;
    
    public int GetID()
    {
        // TODO 임시 처리
        return 0;
    }

    public string GetDataName()
    {
        // TODO 임시 처리
        return null;
    }
}