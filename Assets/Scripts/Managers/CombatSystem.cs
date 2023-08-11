using System;
using System.Collections.Generic;
using Character;
using Character.Hero;
using Character.Monster;
using Effects;
using UnityEngine;

namespace Managers
{
    public static class CombatSystemClient
    {
        public static Monster ms;
        public static void OnDamageCalculatedDataReceived(int data)
        {
            Debug.Log($"Enemy's remain Hp is {data}");
        }

        public static void OnSkillUsed(bool data)
        {
            Debug.Log(data ? "Skill is ready" : "Skill is not ready");
        }

        public static void Attack(Hero hero)
        {
            ms.currentHp -= hero.heroData.skills[0].skillDamage;
            ms.StateMachine.SetTrigger("Damaged");
            if (ms.currentHp <= 0)
            {
                ms.StateMachine.SetBool("Die", true);
                GameObject gold = GameObject.Instantiate(GameManager.Instance.GoldPrefab);
                gold.GetComponent<GoldDropAnimation>().StartUpdate(ms);
                GameManager.Instance.ChangesUserGold(ms);
            }
            
        }
    }

    public static class CombatSystemServer
    {
        public static int VerifyDamage(int damage, int enemyHp)
        {
            return damage - enemyHp;
        }

        public static bool VerifySkillCoolTime(UserData userData, int characterId, int skillId)
        {
            // 현재 시간 - (플레이어의 마지막 스킬 사용 시간 + 쿨타임) > 0 ==> 사용 가능
            // ex) 쿨이 2분인 스킬일 경우 1시 59분에 스킬을 쓰면 2시 01분에 쓸수있음
            // ex) 02:00 - (01:59 + cooltime) > 0 값이 양수면 사용 가능.
            // 쿨타임 표시는 클라에서만...
            
            if (userData.LastSkillUsedTimes.TryGetValue(characterId, out var skillCoolDown)&&
                skillCoolDown.TryGetValue(skillId, out var cooldownEndTime))
            {
                if ((DateTime.Now - (cooldownEndTime +
                     TimeSpan.FromSeconds(GameManager.Instance.skilldata.Datas[skillId].cooldownDuration))).Ticks > 0)
                {
                    //서버에서 기록도 같이..
                    userData.LastSkillUsedTimes[characterId][skillId] = DateTime.Now;
                    return true;
                }
                
#if UNITY_EDITOR
                Debug.Log($"Skill is not ready. SkillName = {GameManager.Instance.skilldata.Datas[skillId].skillName}");
#endif
                return false;
            }
            
#if UNITY_EDITOR
            Debug.Log("Skill Data is not verified.");
#endif
            return false;
        }
    }

    public struct SkillData
    {
        public List<Skill> Datas;
    }
}
