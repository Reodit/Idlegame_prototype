using FSM;
using Managers;
using UnityEngine;

namespace Character.Hero
{
    public class Hero : CharacterBase
    {
        public Animator animator;
        public SpriteRenderer SpriteRenderer;
    
        // TODO 나중에 커스터마이징 할 때 사용
        // public SpriteRenderer[] Customize 
    
        public StateMachine<Hero> StateMachine;
        protected int InstanceId;
        public HeroData heroData;
        public float cooldownTest;
        public override void Init()
        {
            // symbol --> tag
            animator = GetComponentInChildren<Animator>();
            InitStateMachine();

            skillid = InstanceId.ToString() + "0";
            // Key = instanceId + SkillID
            // 이후 DB에서 쿨타임 정보를 받아 1회 할당 
            CoolTimeManager.Instance.RegisterCoolTime(skillid, cooldownTest);
        }
        
        // TODO SkillData 클래스 작성 후 리펙토링
        public string skillid;

        public bool IsCoolTimeFinished()
        {
            bool temp = CoolTimeManager.Instance.IsCoolTimeFinished(skillid);

            if (temp)
            {
                UseSkill();
            }
            
            return temp;
        }

        public void UseSkill()
        {
            CoolTimeManager.Instance.Use(skillid);
        }

        protected virtual void InitStateMachine() { }

        // 이후 서버가 생겼을 때 사용
        /*public void RequestAttack()
           {
               ThreadedDataRequester.RequestData(
                   () => CombatSystemServer.VerifySkillCoolTime(GameManager.Instance.UserData, 0, 0), 
                   Attack);
           }*/
    
        public void Attack()
        {
            // 공격 속도 => 초당 공격횟수
            float attackSpeed = heroData.skills[0].cooldownDuration >= 1 ? 
                1 : 1 / heroData.skills[0].cooldownDuration;
        
            // 애니메이션
            animator.SetFloat("AttackSpeed", attackSpeed);
            animator.SetFloat("AttackSpeed", attackSpeed);
        
            animator.SetTrigger("Attack");
            CombatSystemClient.Attack(this);
        }

        // 스킬은 캔슬 없는 것으로?
        public void UseSkill(int skillId)
        {
            string triggerName = $"Skill{skillId}";
            animator.SetInteger(triggerName, 1);
        }

        public void UseSkillOff(int skillId)
        {
            string triggerName = $"Skill{skillId}";
            animator.SetInteger(triggerName, 0);
        }
    
        public void Move()
        {
            animator.SetBool("Run", true);
        }

        public void MoveOff()
        {
            animator.SetBool("Run", false);
        }
    }
}
